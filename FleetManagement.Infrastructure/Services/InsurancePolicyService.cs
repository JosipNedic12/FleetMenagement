using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class InsurancePolicyService : IInsurancePolicyService
{
    private readonly IInsurancePolicyRepository _repo;

    public InsurancePolicyService(IInsurancePolicyRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<InsurancePolicyDto>> GetAllAsync()
    {
        var policies = await _repo.GetAllAsync();
        return policies.Select(MapToDto);
    }

    public async Task<IEnumerable<InsurancePolicyDto>> GetActiveAsync()
    {
        var policies = await _repo.GetActiveAsync();
        return policies.Select(MapToDto);
    }

    public async Task<InsurancePolicyDto?> GetByIdAsync(int id)
    {
        var policy = await _repo.GetByIdAsync(id);
        if (policy == null) throw new NotFoundException($"Insurance policy with id {id} was not found.");
        return MapToDto(policy);
    }

    public async Task<IEnumerable<InsurancePolicyDto>> GetByVehicleIdAsync(int vehicleId)
    {
        var policies = await _repo.GetByVehicleIdAsync(vehicleId);
        return policies.Select(MapToDto);
    }

    public async Task<InsurancePolicyDto> CreateAsync(CreateInsurancePolicyDto dto)
    {
        var policy = new InsurancePolicy
        {
            VehicleId = dto.VehicleId,
            PolicyNumber = dto.PolicyNumber.Trim(),
            Insurer = dto.Insurer.Trim(),
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo,
            Premium = dto.Premium,
            CoverageNotes = dto.CoverageNotes
        };

        var created = await _repo.CreateAsync(policy);
        return MapToDto(created);
    }

    public async Task<InsurancePolicyDto?> UpdateAsync(int id, UpdateInsurancePolicyDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new NotFoundException($"Insurance policy with id {id} was not found.");

        var updated = await _repo.UpdateAsync(id, new InsurancePolicy
        {
            Insurer = dto.Insurer ?? existing.Insurer,
            ValidFrom = dto.ValidFrom ?? existing.ValidFrom,
            ValidTo = dto.ValidTo ?? existing.ValidTo,
            Premium = dto.Premium ?? existing.Premium,
            CoverageNotes = dto.CoverageNotes ?? existing.CoverageNotes
        });

        if (updated == null) throw new NotFoundException($"Insurance policy with id {id} was not found.");
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Insurance policy with id {id} was not found.");
        return true;
    }

    private static InsurancePolicyDto MapToDto(InsurancePolicy p) => new()
    {
        PolicyId = p.PolicyId,
        VehicleId = p.VehicleId,
        RegistrationNumber = p.Vehicle?.RegistrationNumber ?? string.Empty,
        VehicleMake = p.Vehicle?.Make?.Name ?? string.Empty,
        VehicleModel = p.Vehicle?.Model?.Name ?? string.Empty,
        PolicyNumber = p.PolicyNumber,
        Insurer = p.Insurer,
        ValidFrom = p.ValidFrom,
        ValidTo = p.ValidTo,
        Premium = p.Premium,
        CoverageNotes = p.CoverageNotes,
        IsActive = p.ValidTo >= DateOnly.FromDateTime(DateTime.UtcNow)
    };
}
