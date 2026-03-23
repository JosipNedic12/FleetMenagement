using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IInsurancePolicyService
{
    Task<IEnumerable<InsurancePolicyDto>> GetAllAsync();
    Task<IEnumerable<InsurancePolicyDto>> GetActiveAsync();
    Task<InsurancePolicyDto?> GetByIdAsync(int id);
    Task<IEnumerable<InsurancePolicyDto>> GetByVehicleIdAsync(int vehicleId);
    Task<InsurancePolicyDto> CreateAsync(CreateInsurancePolicyDto dto);
    Task<InsurancePolicyDto?> UpdateAsync(int id, UpdateInsurancePolicyDto dto);
    Task<bool> DeleteAsync(int id);
}
