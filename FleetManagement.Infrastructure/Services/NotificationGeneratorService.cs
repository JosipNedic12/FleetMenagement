using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;
using FleetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FleetManagement.Infrastructure.Services;

public class NotificationGeneratorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationGeneratorService> _logger;

    public NotificationGeneratorService(IServiceScopeFactory scopeFactory, ILogger<NotificationGeneratorService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromHours(24));
        do
        {
            try
            {
                await GenerateNotificationsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating notifications");
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task GenerateNotificationsAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
        var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

        var now = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);
        var cutoff24h = now.AddHours(-24);

        // ── Target users ──────────────────────────────────────────────
        var adminManagerIds = await db.AppUsers
            .AsNoTracking()
            .Where(u => u.IsActive && (u.Role == "Admin" || u.Role == "FleetManager"))
            .Select(u => u.UserId)
            .ToListAsync(ct);

        // ReadOnly/Driver users mapped to their DriverId (for assignment-only notifications)
        var driverUsers = await db.AppUsers
            .AsNoTracking()
            .Where(u => u.IsActive && (u.Role == "ReadOnly" || u.Role == "Driver"))
            .Join(db.Drivers.AsNoTracking(),
                u => u.EmployeeId,
                d => d.EmployeeId,
                (u, d) => new { u.UserId, d.DriverId })
            .ToListAsync(ct);

        if (adminManagerIds.Count == 0 && driverUsers.Count == 0) return;

        // ── Generate notification templates ───────────────────────────
        var templates = new List<NotificationTemplate>();

        await AddExpiredInsurance(db, today, templates, ct);
        await AddExpiringInsurance(db, today, templates, ct);
        await AddExpiringInspections(db, today, templates, ct);
        await AddFailedInspections(db, cutoff24h, templates, ct);
        await AddOverdueMaintenance(db, now, templates, ct);
        await AddCompletedMaintenance(db, cutoff24h, templates, ct);
        await AddUnpaidFines(db, templates, ct);
        await AddNewAssignments(db, cutoff24h, templates, ct);
        await AddAccidents(db, cutoff24h, templates, ct);

        // ── Dedup + expand to target users ────────────────────────────
        var toCreate = new List<Notification>();

        foreach (var t in templates)
        {
            var isDuplicate = await repo.ExistsRecentAsync(
                t.RelatedEntityType, t.RelatedEntityId, t.Title, cutoff24h);

            if (isDuplicate) continue;

            if (t.AssignmentOnly)
            {
                // ReadOnly/Driver users: only if assignment matches their DriverId
                foreach (var du in driverUsers.Where(du => du.DriverId == t.TargetDriverId))
                {
                    toCreate.Add(BuildNotification(t, du.UserId, now));
                }
                // Also send to admins/managers
                foreach (var uid in adminManagerIds)
                {
                    toCreate.Add(BuildNotification(t, uid, now));
                }
            }
            else
            {
                // Admin/FleetManager only
                foreach (var uid in adminManagerIds)
                {
                    toCreate.Add(BuildNotification(t, uid, now));
                }
            }
        }

        if (toCreate.Count > 0)
        {
            await repo.CreateManyAsync(toCreate);
            _logger.LogInformation("Created {Count} notifications", toCreate.Count);
        }
    }

    // ── Notification checks ──────────────────────────────────────────

    private static async Task AddExpiredInsurance(FleetDbContext db, DateOnly today,
        List<NotificationTemplate> list, CancellationToken ct)
    {
        var items = await db.InsurancePolicies.AsNoTracking()
            .Include(p => p.Vehicle)
            .Where(p => p.ValidTo < today && !p.Vehicle.IsDeleted)
            .ToListAsync(ct);

        foreach (var p in items)
        {
            var days = today.DayNumber - p.ValidTo.DayNumber;
            list.Add(new NotificationTemplate
            {
                Title = "Insurance Expired",
                Message = $"Vehicle {p.Vehicle.RegistrationNumber} insurance policy ({p.PolicyNumber}) expired {days} day(s) ago. Renew immediately.",
                Type = "danger",
                RelatedEntityType = "insurance",
                RelatedEntityId = p.PolicyId
            });
        }
    }

    private static async Task AddExpiringInsurance(FleetDbContext db, DateOnly today,
        List<NotificationTemplate> list, CancellationToken ct)
    {
        var limit = today.AddDays(30);
        var items = await db.InsurancePolicies.AsNoTracking()
            .Include(p => p.Vehicle)
            .Where(p => p.ValidTo >= today && p.ValidTo <= limit && !p.Vehicle.IsDeleted)
            .ToListAsync(ct);

        foreach (var p in items)
        {
            var days = p.ValidTo.DayNumber - today.DayNumber;
            list.Add(new NotificationTemplate
            {
                Title = "Insurance Expiring Soon",
                Message = $"Vehicle {p.Vehicle.RegistrationNumber} insurance ({p.PolicyNumber}) expires in {days} day(s).",
                Type = "warning",
                RelatedEntityType = "insurance",
                RelatedEntityId = p.PolicyId
            });
        }
    }

    private static async Task AddExpiringInspections(FleetDbContext db, DateOnly today,
        List<NotificationTemplate> list, CancellationToken ct)
    {
        var limit = today.AddDays(30);
        var items = await db.Inspections.AsNoTracking()
            .Include(i => i.Vehicle)
            .Where(i => i.ValidTo != null && i.ValidTo >= today && i.ValidTo <= limit && !i.Vehicle.IsDeleted)
            .ToListAsync(ct);

        foreach (var i in items)
        {
            var days = i.ValidTo!.Value.DayNumber - today.DayNumber;
            list.Add(new NotificationTemplate
            {
                Title = "Inspection Due Soon",
                Message = $"Vehicle {i.Vehicle.RegistrationNumber} technical inspection expires in {days} day(s).",
                Type = "warning",
                RelatedEntityType = "inspection",
                RelatedEntityId = i.InspectionId
            });
        }
    }

    private static async Task AddFailedInspections(FleetDbContext db, DateTime cutoff,
        List<NotificationTemplate> list, CancellationToken ct)
    {
        var items = await db.Inspections.AsNoTracking()
            .Include(i => i.Vehicle)
            .Where(i => i.Result == "failed" && i.CreatedAt >= cutoff && !i.Vehicle.IsDeleted)
            .ToListAsync(ct);

        foreach (var i in items)
        {
            list.Add(new NotificationTemplate
            {
                Title = "Inspection Failed",
                Message = $"Vehicle {i.Vehicle.RegistrationNumber} failed technical inspection.",
                Type = "danger",
                RelatedEntityType = "inspection",
                RelatedEntityId = i.InspectionId
            });
        }
    }

    private static async Task AddOverdueMaintenance(FleetDbContext db, DateTime now,
        List<NotificationTemplate> list, CancellationToken ct)
    {
        var items = await db.MaintenanceOrders.AsNoTracking()
            .Include(o => o.Vehicle)
            .Where(o => (o.Status == "open" || o.Status == "in_progress")
                        && o.ScheduledAt != null && o.ScheduledAt < now
                        && !o.Vehicle.IsDeleted)
            .ToListAsync(ct);

        foreach (var o in items)
        {
            var days = (int)(now - o.ScheduledAt!.Value).TotalDays;
            list.Add(new NotificationTemplate
            {
                Title = "Overdue Maintenance Order",
                Message = $"Work order #{o.OrderId} ({o.Vehicle.RegistrationNumber}) is overdue by {days} day(s).",
                Type = "warning",
                RelatedEntityType = "maintenance",
                RelatedEntityId = o.OrderId
            });
        }
    }

    private static async Task AddCompletedMaintenance(FleetDbContext db, DateTime cutoff,
        List<NotificationTemplate> list, CancellationToken ct)
    {
        var items = await db.MaintenanceOrders.AsNoTracking()
            .Include(o => o.Vehicle)
            .Where(o => o.Status == "closed" && o.ClosedAt != null && o.ClosedAt >= cutoff && !o.Vehicle.IsDeleted)
            .ToListAsync(ct);

        foreach (var o in items)
        {
            list.Add(new NotificationTemplate
            {
                Title = "Maintenance Completed",
                Message = $"Work order #{o.OrderId} ({o.Vehicle.RegistrationNumber}) was closed successfully.",
                Type = "success",
                RelatedEntityType = "maintenance",
                RelatedEntityId = o.OrderId
            });
        }
    }

    private static async Task AddUnpaidFines(FleetDbContext db,
        List<NotificationTemplate> list, CancellationToken ct)
    {
        var items = await db.Fines.AsNoTracking()
            .Include(f => f.Vehicle)
            .Where(f => f.PaidAt == null && !f.Vehicle.IsDeleted)
            .ToListAsync(ct);

        foreach (var f in items)
        {
            list.Add(new NotificationTemplate
            {
                Title = "Unpaid Fine",
                Message = $"Vehicle {f.Vehicle.RegistrationNumber} has an unpaid fine of {f.Amount:N2} EUR ({f.Reason}).",
                Type = "danger",
                RelatedEntityType = "fine",
                RelatedEntityId = f.FineId
            });
        }
    }

    private static async Task AddNewAssignments(FleetDbContext db, DateTime cutoff,
        List<NotificationTemplate> list, CancellationToken ct)
    {
        var items = await db.VehicleAssignments.AsNoTracking()
            .Include(a => a.Vehicle)
            .Include(a => a.Driver).ThenInclude(d => d.Employee)
            .Where(a => a.CreatedAt >= cutoff && !a.Vehicle.IsDeleted)
            .ToListAsync(ct);

        foreach (var a in items)
        {
            var name = $"{a.Driver.Employee.FirstName} {a.Driver.Employee.LastName}";
            list.Add(new NotificationTemplate
            {
                Title = "New Driver Assigned",
                Message = $"{name} has been assigned to vehicle {a.Vehicle.RegistrationNumber}.",
                Type = "info",
                RelatedEntityType = "vehicle",
                RelatedEntityId = a.VehicleId,
                AssignmentOnly = true,
                TargetDriverId = a.DriverId
            });
        }
    }

    private static async Task AddAccidents(FleetDbContext db, DateTime cutoff,
        List<NotificationTemplate> list, CancellationToken ct)
    {
        var items = await db.Accidents.AsNoTracking()
            .Include(a => a.Vehicle)
            .Where(a => a.CreatedAt >= cutoff && !a.Vehicle.IsDeleted)
            .ToListAsync(ct);

        foreach (var a in items)
        {
            var damage = a.DamageEstimate.HasValue
                ? $" — estimated damage: {a.DamageEstimate.Value:N2} EUR"
                : "";
            list.Add(new NotificationTemplate
            {
                Title = "Accident Reported",
                Message = $"Accident reported for vehicle {a.Vehicle.RegistrationNumber} ({a.Severity}){damage}.",
                Type = "danger",
                RelatedEntityType = "accident",
                RelatedEntityId = a.AccidentId
            });
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────

    private static Notification BuildNotification(NotificationTemplate t, int userId, DateTime now) => new()
    {
        UserId = userId,
        Title = t.Title,
        Message = t.Message,
        Type = t.Type,
        RelatedEntityType = t.RelatedEntityType,
        RelatedEntityId = t.RelatedEntityId,
        CreatedAt = now,
        IsRead = false
    };

    private class NotificationTemplate
    {
        public string Title { get; init; } = "";
        public string Message { get; init; } = "";
        public string Type { get; init; } = "info";
        public string RelatedEntityType { get; init; } = "";
        public int RelatedEntityId { get; init; }
        public bool AssignmentOnly { get; init; }
        public int? TargetDriverId { get; init; }
    }
}
