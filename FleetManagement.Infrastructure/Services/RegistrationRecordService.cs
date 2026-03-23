using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class RegistrationRecordService : IRegistrationRecordService
{
    private readonly IRegistrationRecordRepository _repo;

    public RegistrationRecordService(IRegistrationRecordRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<RegistrationRecordDto>> GetAllAsync()
    {
        var records = await _repo.GetAllAsync();
        return records.Select(MapToDto);
    }

    public async Task<RegistrationRecordDto?> GetByIdAsync(int id)
    {
        var record = await _repo.GetByIdAsync(id);
        if (record == null) throw new NotFoundException($"Registration record with id {id} was not found.");
        return MapToDto(record);
    }

    public async Task<IEnumerable<RegistrationRecordDto>> GetByVehicleIdAsync(int vehicleId)
    {
        var records = await _repo.GetByVehicleIdAsync(vehicleId);
        return records.Select(MapToDto);
    }

    public async Task<RegistrationRecordDto?> GetCurrentByVehicleIdAsync(int vehicleId)
    {
        var record = await _repo.GetCurrentByVehicleIdAsync(vehicleId);
        if (record == null) throw new NotFoundException($"No current registration record found for vehicle {vehicleId}.");
        return MapToDto(record);
    }

    public async Task<RegistrationRecordDto> CreateAsync(CreateRegistrationRecordDto dto)
    {
        var record = new RegistrationRecord
        {
            VehicleId = dto.VehicleId,
            RegistrationNumber = dto.RegistrationNumber.Trim(),
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo,
            Fee = dto.Fee,
            Notes = dto.Notes
        };

        var created = await _repo.CreateAsync(record);
        return MapToDto(created);
    }

    public async Task<RegistrationRecordDto?> UpdateAsync(int id, UpdateRegistrationRecordDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new NotFoundException($"Registration record with id {id} was not found.");

        var updated = await _repo.UpdateAsync(id, new RegistrationRecord
        {
            RegistrationNumber = dto.RegistrationNumber ?? existing.RegistrationNumber,
            ValidFrom = dto.ValidFrom ?? existing.ValidFrom,
            ValidTo = dto.ValidTo ?? existing.ValidTo,
            Fee = dto.Fee ?? existing.Fee,
            Notes = dto.Notes ?? existing.Notes
        });

        if (updated == null) throw new NotFoundException($"Registration record with id {id} was not found.");
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Registration record with id {id} was not found.");
        return true;
    }

    private static RegistrationRecordDto MapToDto(RegistrationRecord r)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return new()
        {
            RegistrationId = r.RegistrationId,
            VehicleId = r.VehicleId,
            VehicleRegistrationNumber = r.Vehicle?.RegistrationNumber ?? string.Empty,
            VehicleMake = r.Vehicle?.Make?.Name ?? string.Empty,
            VehicleModel = r.Vehicle?.Model?.Name ?? string.Empty,
            RegistrationNumber = r.RegistrationNumber,
            ValidFrom = r.ValidFrom,
            ValidTo = r.ValidTo,
            Fee = r.Fee,
            Notes = r.Notes,
            IsActive = r.ValidFrom <= today && r.ValidTo >= today
        };
    }
}
