using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IInspectionService
{
    Task<IEnumerable<InspectionDto>> GetAllAsync();
    Task<InspectionDto?> GetByIdAsync(int id);
    Task<IEnumerable<InspectionDto>> GetByVehicleIdAsync(int vehicleId);
    Task<InspectionDto?> GetLatestByVehicleIdAsync(int vehicleId);
    Task<InspectionDto> CreateAsync(CreateInspectionDto dto);
    Task<InspectionDto?> UpdateAsync(int id, UpdateInspectionDto dto);
    Task<bool> DeleteAsync(int id);
}
