using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IOdometerLogService
{
    Task<IEnumerable<OdometerLogDto>> GetAllAsync();
    Task<OdometerLogDto?> GetByIdAsync(int id);
    Task<IEnumerable<OdometerLogDto>> GetByVehicleIdAsync(int vehicleId);
    Task<OdometerLogDto> CreateAsync(CreateOdometerLogDto dto);
    Task<bool> DeleteAsync(int id);
}
