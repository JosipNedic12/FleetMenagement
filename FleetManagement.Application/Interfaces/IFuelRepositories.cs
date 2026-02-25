using FleetManagement.Domain.Entities;

namespace FleetManagement.Application.Interfaces;

public interface IFuelCardRepository
{
    Task<IEnumerable<FuelCard>> GetAllAsync();
    Task<FuelCard?> GetByIdAsync(int id);
    Task<IEnumerable<FuelCard>> GetByVehicleIdAsync(int vehicleId);
    Task<FuelCard> CreateAsync(FuelCard card);
    Task<FuelCard?> UpdateAsync(int id, FuelCard updated);
    Task<bool> DeleteAsync(int id); // soft delete via IsActive = false
}

public interface IFuelTransactionRepository
{
    Task<IEnumerable<FuelTransaction>> GetAllAsync();
    Task<IEnumerable<FuelTransaction>> GetByVehicleIdAsync(int vehicleId);
    Task<FuelTransaction?> GetByIdAsync(int id);
    Task<FuelTransaction> CreateAsync(FuelTransaction transaction);
    Task<bool> DeleteAsync(int id);
    Task<bool> MarkSuspiciousAsync(int id, bool isSuspicious);
}