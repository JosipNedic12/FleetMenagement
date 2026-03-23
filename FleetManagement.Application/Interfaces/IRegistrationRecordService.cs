using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IRegistrationRecordService
{
    Task<IEnumerable<RegistrationRecordDto>> GetAllAsync();
    Task<RegistrationRecordDto?> GetByIdAsync(int id);
    Task<IEnumerable<RegistrationRecordDto>> GetByVehicleIdAsync(int vehicleId);
    Task<RegistrationRecordDto?> GetCurrentByVehicleIdAsync(int vehicleId);
    Task<RegistrationRecordDto> CreateAsync(CreateRegistrationRecordDto dto);
    Task<RegistrationRecordDto?> UpdateAsync(int id, UpdateRegistrationRecordDto dto);
    Task<bool> DeleteAsync(int id);
}
