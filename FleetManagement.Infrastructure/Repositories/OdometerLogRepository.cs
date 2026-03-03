using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;
using FleetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure.Repositories;

public class OdometerLogRepository : IOdometerLogRepository
{
    private readonly FleetDbContext _context;
    public OdometerLogRepository(FleetDbContext context) => _context = context;

    public async Task<IEnumerable<OdometerLog>> GetAllAsync()
    {
        return await _context.OdometerLogs
        .Include(x => x.Vehicle)
        .ToListAsync();
    }
    public async Task<IEnumerable<OdometerLog>> GetByVehicleIdAsync(int vehicleId) =>
        await _context.OdometerLogs
            .Include(l => l.Vehicle)
            .Where(l => l.VehicleId == vehicleId)
            .OrderByDescending(l => l.LogDate)
            .ToListAsync();

    public async Task<OdometerLog?> GetByIdAsync(int logId) =>
        await _context.OdometerLogs
            .Include(l => l.Vehicle)
            .FirstOrDefaultAsync(l => l.LogId == logId);

    public async Task<OdometerLog> CreateAsync(OdometerLog log)
    {
        log.CreatedAt = DateTime.UtcNow;
        _context.OdometerLogs.Add(log);

        // Auto-update Vehicle.CurrentOdometerKm if this reading is higher
        var vehicle = await _context.Vehicles.FindAsync(log.VehicleId);
        if (vehicle != null && log.OdometerKm > vehicle.CurrentOdometerKm)
        {
            vehicle.CurrentOdometerKm = log.OdometerKm;
            vehicle.ModifiedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return await _context.OdometerLogs
            .Include(l => l.Vehicle)
            .FirstAsync(l => l.LogId == log.LogId);
    }

    public async Task<bool> DeleteAsync(int logId)
    {
        var log = await _context.OdometerLogs
            .Include(l => l.Vehicle)
            .FirstOrDefaultAsync(l => l.LogId == logId);

        if (log == null) return false;

        _context.OdometerLogs.Remove(log);

        // Recalculate vehicle odometer from remaining logs
        var newMax = await _context.OdometerLogs
            .Where(l => l.VehicleId == log.VehicleId && l.LogId != logId)
            .MaxAsync(l => (int?)l.OdometerKm) ?? 0;

        if (log.Vehicle != null)
        {
            log.Vehicle.CurrentOdometerKm = newMax;
            log.Vehicle.ModifiedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int?> GetLatestOdometerAsync(int vehicleId) =>
        await _context.OdometerLogs
            .Where(l => l.VehicleId == vehicleId)
            .MaxAsync(l => (int?)l.OdometerKm);
}


