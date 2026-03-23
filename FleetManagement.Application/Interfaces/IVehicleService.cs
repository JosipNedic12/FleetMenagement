using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDto>> GetAllAsync();
    Task<VehicleDto?> GetByIdAsync(int id);
    Task<VehicleDto> CreateAsync(CreateVehicleDto dto);
    Task<VehicleDto?> UpdateAsync(int id, UpdateVehicleDto dto);
    Task<bool> DeleteAsync(int id);
}
