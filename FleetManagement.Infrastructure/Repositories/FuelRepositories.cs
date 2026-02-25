using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;
using FleetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure.Repositories;

// -------------------------------------------------------
// FuelCardRepository
// -------------------------------------------------------
public class FuelCardRepository : IFuelCardRepository
{
    private readonly FleetDbContext _context;
    public FuelCardRepository(FleetDbContext context) => _context = context;

    public async Task<IEnumerable<FuelCard>> GetAllAsync() =>
        await _context.FuelCards
            .Include(c => c.AssignedVehicle)
            .OrderBy(c => c.CardNumber)
            .ToListAsync();

    public async Task<FuelCard?> GetByIdAsync(int id) =>
        await _context.FuelCards
            .Include(c => c.AssignedVehicle)
            .FirstOrDefaultAsync(c => c.FuelCardId == id);

    public async Task<IEnumerable<FuelCard>> GetByVehicleIdAsync(int vehicleId) =>
        await _context.FuelCards
            .Include(c => c.AssignedVehicle)
            .Where(c => c.AssignedVehicleId == vehicleId)
            .ToListAsync();

    public async Task<FuelCard> CreateAsync(FuelCard card)
    {
        card.CreatedAt = DateTime.UtcNow;
        _context.FuelCards.Add(card);
        await _context.SaveChangesAsync();
        return await _context.FuelCards
            .Include(c => c.AssignedVehicle)
            .FirstAsync(c => c.FuelCardId == card.FuelCardId);
    }

    public async Task<FuelCard?> UpdateAsync(int id, FuelCard updated)
    {
        var card = await _context.FuelCards.FindAsync(id);
        if (card == null) return null;

        card.Provider = updated.Provider;
        card.AssignedVehicleId = updated.AssignedVehicleId;
        card.ValidFrom = updated.ValidFrom;
        card.ValidTo = updated.ValidTo;
        card.IsActive = updated.IsActive;
        card.Notes = updated.Notes;
        card.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await _context.FuelCards
            .Include(c => c.AssignedVehicle)
            .FirstAsync(c => c.FuelCardId == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var card = await _context.FuelCards.FindAsync(id);
        if (card == null) return false;

        card.IsActive = false;
        card.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}

// -------------------------------------------------------
// FuelTransactionRepository
// -------------------------------------------------------
public class FuelTransactionRepository : IFuelTransactionRepository
{
    private readonly FleetDbContext _context;
    public FuelTransactionRepository(FleetDbContext context) => _context = context;

    private IQueryable<FuelTransaction> BaseQuery() =>
        _context.FuelTransactions
            .Include(t => t.Vehicle)
            .Include(t => t.FuelCard)
            .Include(t => t.FuelType);

    public async Task<IEnumerable<FuelTransaction>> GetAllAsync() =>
        await BaseQuery()
            .OrderByDescending(t => t.PostedAt)
            .ToListAsync();

    public async Task<IEnumerable<FuelTransaction>> GetByVehicleIdAsync(int vehicleId) =>
        await BaseQuery()
            .Where(t => t.VehicleId == vehicleId)
            .OrderByDescending(t => t.PostedAt)
            .ToListAsync();

    public async Task<FuelTransaction?> GetByIdAsync(int id) =>
        await BaseQuery().FirstOrDefaultAsync(t => t.TransactionId == id);

    public async Task<FuelTransaction> CreateAsync(FuelTransaction transaction)
    {
        transaction.CreatedAt = DateTime.UtcNow;
        _context.FuelTransactions.Add(transaction);

        // Auto-update vehicle odometer if provided and higher than current
        if (transaction.OdometerKm.HasValue)
        {
            var vehicle = await _context.Vehicles.FindAsync(transaction.VehicleId);
            if (vehicle != null && transaction.OdometerKm.Value > vehicle.CurrentOdometerKm)
            {
                vehicle.CurrentOdometerKm = transaction.OdometerKm.Value;
                vehicle.ModifiedAt = DateTime.UtcNow;

                // Also log to odometer_log
                _context.OdometerLogs.Add(new OdometerLog
                {
                    VehicleId = transaction.VehicleId,
                    OdometerKm = transaction.OdometerKm.Value,
                    LogDate = DateOnly.FromDateTime(transaction.PostedAt),
                    Notes = $"Auto-logged from fuel transaction #{transaction.TransactionId}",
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync();
        return await BaseQuery().FirstAsync(t => t.TransactionId == transaction.TransactionId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var transaction = await _context.FuelTransactions.FindAsync(id);
        if (transaction == null) return false;

        _context.FuelTransactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkSuspiciousAsync(int id, bool isSuspicious)
    {
        var transaction = await _context.FuelTransactions.FindAsync(id);
        if (transaction == null) return false;

        transaction.IsSuspicious = isSuspicious;
        await _context.SaveChangesAsync();
        return true;
    }
}