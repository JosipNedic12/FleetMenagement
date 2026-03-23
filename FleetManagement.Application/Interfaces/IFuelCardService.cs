using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IFuelCardService
{
    Task<IEnumerable<FuelCardDto>> GetAllAsync();
    Task<FuelCardDto?> GetByIdAsync(int id);
    Task<IEnumerable<FuelCardDto>> GetByVehicleIdAsync(int vehicleId);
    Task<FuelCardDto> CreateAsync(CreateFuelCardDto dto);
    Task<FuelCardDto?> UpdateAsync(int id, UpdateFuelCardDto dto);
    Task<bool> DeleteAsync(int id);
}
