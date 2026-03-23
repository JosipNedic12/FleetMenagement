using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IDriverService
{
    Task<IEnumerable<DriverDto>> GetAllAsync();
    Task<DriverDto?> GetByIdAsync(int id);
    Task<DriverDto> CreateAsync(CreateDriverDto dto);
    Task<DriverDto?> UpdateAsync(int id, UpdateDriverDto dto);
    Task<bool> DeleteAsync(int id);
}
