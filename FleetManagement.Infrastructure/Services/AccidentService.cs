using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class AccidentService : IAccidentService
{
    private readonly IAccidentRepository _repo;

    public AccidentService(IAccidentRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<AccidentDto>> GetAllAsync()
    {
        var accidents = await _repo.GetAllAsync();
        return accidents.Select(MapToDto);
    }

    public async Task<AccidentDto?> GetByIdAsync(int id)
    {
        var accident = await _repo.GetByIdAsync(id);
        if (accident == null) throw new NotFoundException($"Accident with id {id} was not found.");
        return MapToDto(accident);
    }

    public async Task<IEnumerable<AccidentDto>> GetByVehicleIdAsync(int vehicleId)
    {
        var accidents = await _repo.GetByVehicleIdAsync(vehicleId);
        return accidents.Select(MapToDto);
    }

    public async Task<IEnumerable<AccidentDto>> GetByDriverIdAsync(int driverId)
    {
        var accidents = await _repo.GetByDriverIdAsync(driverId);
        return accidents.Select(MapToDto);
    }

    public async Task<AccidentDto> CreateAsync(CreateAccidentDto dto)
    {
        var accident = new Accident
        {
            VehicleId = dto.VehicleId,
            DriverId = dto.DriverId,
            OccurredAt = dto.OccurredAt,
            Severity = dto.Severity,
            Description = dto.Description,
            DamageEstimate = dto.DamageEstimate,
            PoliceReport = dto.PoliceReport,
            Notes = dto.Notes
        };

        var created = await _repo.CreateAsync(accident);
        return MapToDto(created);
    }

    public async Task<AccidentDto?> UpdateAsync(int id, UpdateAccidentDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new NotFoundException($"Accident with id {id} was not found.");

        var updated = await _repo.UpdateAsync(id, new Accident
        {
            DriverId = dto.DriverId ?? existing.DriverId,
            OccurredAt = dto.OccurredAt ?? existing.OccurredAt,
            Severity = dto.Severity ?? existing.Severity,
            Description = dto.Description ?? existing.Description,
            DamageEstimate = dto.DamageEstimate ?? existing.DamageEstimate,
            PoliceReport = dto.PoliceReport ?? existing.PoliceReport,
            Notes = dto.Notes ?? existing.Notes
        });

        if (updated == null) throw new NotFoundException($"Accident with id {id} was not found.");
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Accident with id {id} was not found.");
        return true;
    }

    private static AccidentDto MapToDto(Accident a) => new()
    {
        AccidentId = a.AccidentId,
        VehicleId = a.VehicleId,
        RegistrationNumber = a.Vehicle?.RegistrationNumber ?? string.Empty,
        VehicleMake = a.Vehicle?.Make?.Name ?? string.Empty,
        VehicleModel = a.Vehicle?.Model?.Name ?? string.Empty,
        DriverId = a.DriverId,
        DriverName = a.Driver?.Employee != null
            ? $"{a.Driver.Employee.FirstName} {a.Driver.Employee.LastName}"
            : null,
        OccurredAt = a.OccurredAt,
        Severity = a.Severity,
        Description = a.Description,
        DamageEstimate = a.DamageEstimate,
        PoliceReport = a.PoliceReport,
        Notes = a.Notes
    };
}
