using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class FuelCardService : IFuelCardService
{
    private readonly IFuelCardRepository _repo;

    public FuelCardService(IFuelCardRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<FuelCardDto>> GetAllAsync()
    {
        var cards = await _repo.GetAllAsync();
        return cards.Select(MapToDto);
    }

    public async Task<FuelCardDto?> GetByIdAsync(int id)
    {
        var card = await _repo.GetByIdAsync(id);
        if (card == null) throw new NotFoundException($"Fuel card with id {id} was not found.");
        return MapToDto(card);
    }

    public async Task<IEnumerable<FuelCardDto>> GetByVehicleIdAsync(int vehicleId)
    {
        var cards = await _repo.GetByVehicleIdAsync(vehicleId);
        return cards.Select(MapToDto);
    }

    public async Task<FuelCardDto> CreateAsync(CreateFuelCardDto dto)
    {
        var card = new FuelCard
        {
            CardNumber = dto.CardNumber.Trim(),
            Provider = dto.Provider,
            AssignedVehicleId = dto.AssignedVehicleId,
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo,
            Notes = dto.Notes
        };

        var created = await _repo.CreateAsync(card);
        return MapToDto(created);
    }

    public async Task<FuelCardDto?> UpdateAsync(int id, UpdateFuelCardDto dto)
    {
        var updated = await _repo.UpdateAsync(id, new FuelCard
        {
            Provider = dto.Provider,
            AssignedVehicleId = dto.AssignedVehicleId,
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo,
            IsActive = dto.IsActive ?? true,
            Notes = dto.Notes
        });

        if (updated == null) throw new NotFoundException($"Fuel card with id {id} was not found.");
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Fuel card with id {id} was not found.");
        return true;
    }

    private static FuelCardDto MapToDto(FuelCard c) => new()
    {
        FuelCardId = c.FuelCardId,
        CardNumber = c.CardNumber,
        Provider = c.Provider,
        AssignedVehicleId = c.AssignedVehicleId,
        RegistrationNumber = c.AssignedVehicle?.RegistrationNumber,
        VehicleMake = c.AssignedVehicle?.Make?.Name,
        VehicleModel = c.AssignedVehicle?.Model?.Name,
        ValidFrom = c.ValidFrom,
        ValidTo = c.ValidTo,
        IsActive = c.IsActive,
        Notes = c.Notes
    };
}
