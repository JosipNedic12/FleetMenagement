using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IFineService
{
    Task<IEnumerable<FineDto>> GetAllAsync();
    Task<IEnumerable<FineDto>> GetUnpaidAsync();
    Task<FineDto?> GetByIdAsync(int id);
    Task<IEnumerable<FineDto>> GetByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<FineDto>> GetByDriverIdAsync(int driverId);
    Task<FineDto> CreateAsync(CreateFineDto dto);
    Task<FineDto?> UpdateAsync(int id, UpdateFineDto dto);
    Task<FineDto?> MarkPaidAsync(int id, MarkFinePaidDto dto);
    Task<bool> DeleteAsync(int id);
}
