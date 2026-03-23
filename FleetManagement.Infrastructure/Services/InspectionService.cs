using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class InspectionService : IInspectionService
{
    private readonly IInspectionRepository _repo;

    public InspectionService(IInspectionRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<InspectionDto>> GetAllAsync()
    {
        var inspections = await _repo.GetAllAsync();
        return inspections.Select(MapToDto);
    }

    public async Task<InspectionDto?> GetByIdAsync(int id)
    {
        var inspection = await _repo.GetByIdAsync(id);
        if (inspection == null) throw new NotFoundException($"Inspection with id {id} was not found.");
        return MapToDto(inspection);
    }

    public async Task<IEnumerable<InspectionDto>> GetByVehicleIdAsync(int vehicleId)
    {
        var inspections = await _repo.GetByVehicleIdAsync(vehicleId);
        return inspections.Select(MapToDto);
    }

    public async Task<InspectionDto?> GetLatestByVehicleIdAsync(int vehicleId)
    {
        var inspection = await _repo.GetLatestByVehicleIdAsync(vehicleId);
        if (inspection == null) throw new NotFoundException($"No inspection found for vehicle {vehicleId}.");
        return MapToDto(inspection);
    }

    public async Task<InspectionDto> CreateAsync(CreateInspectionDto dto)
    {
        if (dto.Result == "failed" && string.IsNullOrWhiteSpace(dto.Notes))
            throw new InvalidOperationException("Notes are required for failed inspections.");

        var inspection = new Inspection
        {
            VehicleId = dto.VehicleId,
            InspectedAt = dto.InspectedAt,
            ValidTo = dto.ValidTo,
            Result = dto.Result,
            Notes = dto.Notes,
            OdometerKm = dto.OdometerKm
        };

        var created = await _repo.CreateAsync(inspection);
        return MapToDto(created);
    }

    public async Task<InspectionDto?> UpdateAsync(int id, UpdateInspectionDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new NotFoundException($"Inspection with id {id} was not found.");

        var resultToUse = dto.Result ?? existing.Result;
        var notesToUse = dto.Notes ?? existing.Notes;

        if (resultToUse == "failed" && string.IsNullOrWhiteSpace(notesToUse))
            throw new InvalidOperationException("Notes are required for failed inspections.");

        var updated = await _repo.UpdateAsync(id, new Inspection
        {
            InspectedAt = dto.InspectedAt ?? existing.InspectedAt,
            ValidTo = dto.ValidTo ?? existing.ValidTo,
            Result = resultToUse,
            Notes = notesToUse,
            OdometerKm = dto.OdometerKm ?? existing.OdometerKm
        });

        if (updated == null) throw new NotFoundException($"Inspection with id {id} was not found.");
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Inspection with id {id} was not found.");
        return true;
    }

    private static InspectionDto MapToDto(Inspection i)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return new()
        {
            InspectionId = i.InspectionId,
            VehicleId = i.VehicleId,
            RegistrationNumber = i.Vehicle?.RegistrationNumber ?? string.Empty,
            VehicleMake = i.Vehicle?.Make?.Name ?? string.Empty,
            VehicleModel = i.Vehicle?.Model?.Name ?? string.Empty,
            InspectedAt = i.InspectedAt,
            ValidTo = i.ValidTo,
            Result = i.Result,
            Notes = i.Notes,
            OdometerKm = i.OdometerKm,
            IsValid = i.ValidTo.HasValue && i.ValidTo.Value >= today
        };
    }
}
