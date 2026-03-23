using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IFuelTransactionService
{
    Task<IEnumerable<FuelTransactionDto>> GetAllAsync();
    Task<FuelTransactionDto?> GetByIdAsync(int id);
    Task<IEnumerable<FuelTransactionDto>> GetByVehicleIdAsync(int vehicleId);
    Task<FuelTransactionDto> CreateAsync(CreateFuelTransactionDto dto);
    Task<bool> MarkSuspiciousAsync(int id, bool isSuspicious);
    Task<bool> DeleteAsync(int id);
}
