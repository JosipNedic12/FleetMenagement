using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Application.Interfaces;

public interface IOdometerLogRepository
{
    Task<IEnumerable<OdometerLog>> GetAllAsync();
    Task<IEnumerable<OdometerLog>> GetByVehicleIdAsync(int vehicleId);
    Task<OdometerLog?> GetByIdAsync(int logId);
    Task<OdometerLog> CreateAsync(OdometerLog log);
    Task<bool> DeleteAsync(int logId);

    // Returns the highest odometer reading for a vehicle
    Task<int?> GetLatestOdometerAsync(int vehicleId);
}