using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;
using FleetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure.Repositories;

public class VehicleAssignmentRepository : IVehicleAssignmentRepository
{
    private readonly FleetDbContext _context;
    public VehicleAssignmentRepository(FleetDbContext context) => _context = context;

    private IQueryable<VehicleAssignment> BaseQuery() =>
        _context.VehicleAssignments
            .Include(a => a.Vehicle)
                .ThenInclude(v => v.Make)
            .Include(a => a.Vehicle)
                .ThenInclude(v => v.Model)
            .Include(a => a.Driver)
                .ThenInclude(d => d.Employee);

    // Active = AssignedTo is null or in the future
    private static bool IsActive(VehicleAssignment a) =>
        a.AssignedTo == null || a.AssignedTo >= DateOnly.FromDateTime(DateTime.Today);

    public async Task<IEnumerable<VehicleAssignment>> GetAllAsync(bool activeOnly = false)
    {
        var list = await BaseQuery().OrderByDescending(a => a.AssignedFrom).ToListAsync();
        return activeOnly ? list.Where(IsActive) : list;
    }

    public async Task<VehicleAssignment?> GetByIdAsync(int id) =>
        await BaseQuery().FirstOrDefaultAsync(a => a.AssignmentId == id);

    public async Task<IEnumerable<VehicleAssignment>> GetByVehicleIdAsync(int vehicleId) =>
        await BaseQuery()
            .Where(a => a.VehicleId == vehicleId)
            .OrderByDescending(a => a.AssignedFrom)
            .ToListAsync();

    public async Task<IEnumerable<VehicleAssignment>> GetByDriverIdAsync(int driverId) =>
        await BaseQuery()
            .Where(a => a.DriverId == driverId)
            .OrderByDescending(a => a.AssignedFrom)
            .ToListAsync();

    public async Task<VehicleAssignment?> GetActiveByVehicleIdAsync(int vehicleId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return await BaseQuery()
            .FirstOrDefaultAsync(a => a.VehicleId == vehicleId &&
                (a.AssignedTo == null || a.AssignedTo >= today));
    }

    public async Task<bool> VehicleHasActiveAssignmentAsync(int vehicleId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return await _context.VehicleAssignments
            .AnyAsync(a => a.VehicleId == vehicleId &&
                (a.AssignedTo == null || a.AssignedTo >= today));
    }

    public async Task<VehicleAssignment> CreateAsync(VehicleAssignment assignment)
    {
        assignment.CreatedAt = DateTime.UtcNow;
        _context.VehicleAssignments.Add(assignment);
        await _context.SaveChangesAsync();
        return await BaseQuery().FirstAsync(a => a.AssignmentId == assignment.AssignmentId);
    }

    public async Task<VehicleAssignment?> UpdateAsync(int id, UpdateAssignmentData data)
    {
        var assignment = await _context.VehicleAssignments.FindAsync(id);
        if (assignment == null) return null;

        if (data.AssignedTo.HasValue) assignment.AssignedTo = data.AssignedTo;
        if (data.Notes != null) assignment.Notes = data.Notes;
        assignment.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await BaseQuery().FirstAsync(a => a.AssignmentId == id);
    }

    public async Task<bool> EndAssignmentAsync(int id)
    {
        var assignment = await _context.VehicleAssignments.FindAsync(id);
        if (assignment == null) return false;

        var today = DateOnly.FromDateTime(DateTime.Today);
        if (assignment.AssignedTo != null && assignment.AssignedTo < today) return false; // already ended

        assignment.AssignedTo = today;
        assignment.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}