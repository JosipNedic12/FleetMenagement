using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class OdometerLogService : IOdometerLogService
{
    private readonly IOdometerLogRepository _repo;

    public OdometerLogService(IOdometerLogRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<OdometerLogDto>> GetAllAsync()
    {
        var logs = await _repo.GetAllAsync();
        return logs.Select(MapToDto);
    }

    public async Task<OdometerLogDto?> GetByIdAsync(int id)
    {
        var log = await _repo.GetByIdAsync(id);
        if (log == null) throw new NotFoundException($"Odometer log with id {id} was not found.");
        return MapToDto(log);
    }

    public async Task<IEnumerable<OdometerLogDto>> GetByVehicleIdAsync(int vehicleId)
    {
        var logs = await _repo.GetByVehicleIdAsync(vehicleId);
        return logs.Select(MapToDto);
    }

    public async Task<OdometerLogDto> CreateAsync(CreateOdometerLogDto dto)
    {
        var latest = await _repo.GetLatestOdometerAsync(dto.VehicleId);
        if (latest.HasValue && dto.OdometerKm < latest.Value)
            throw new ConflictException($"Odometer reading ({dto.OdometerKm} km) cannot be less than the current reading ({latest.Value} km).");

        var log = new OdometerLog
        {
            VehicleId = dto.VehicleId,
            OdometerKm = dto.OdometerKm,
            LogDate = dto.LogDate,
            Notes = dto.Notes
        };

        var created = await _repo.CreateAsync(log);
        return MapToDto(created);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Odometer log with id {id} was not found.");
        return true;
    }

    private static OdometerLogDto MapToDto(OdometerLog l) => new()
    {
        LogId = l.LogId,
        VehicleId = l.VehicleId,
        RegistrationNumber = l.Vehicle.RegistrationNumber,
        OdometerKm = l.OdometerKm,
        LogDate = l.LogDate,
        Notes = l.Notes,
        CreatedAt = l.CreatedAt
    };
}
