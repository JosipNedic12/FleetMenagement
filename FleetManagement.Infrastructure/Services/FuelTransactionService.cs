using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class FuelTransactionService : IFuelTransactionService
{
    private readonly IFuelTransactionRepository _repo;

    public FuelTransactionService(IFuelTransactionRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<FuelTransactionDto>> GetAllAsync()
    {
        var transactions = await _repo.GetAllAsync();
        return transactions.Select(MapToDto);
    }

    public async Task<FuelTransactionDto?> GetByIdAsync(int id)
    {
        var transaction = await _repo.GetByIdAsync(id);
        if (transaction == null) throw new NotFoundException($"Fuel transaction with id {id} was not found.");
        return MapToDto(transaction);
    }

    public async Task<IEnumerable<FuelTransactionDto>> GetByVehicleIdAsync(int vehicleId)
    {
        var transactions = await _repo.GetByVehicleIdAsync(vehicleId);
        return transactions.Select(MapToDto);
    }

    public async Task<FuelTransactionDto> CreateAsync(CreateFuelTransactionDto dto)
    {
        var transaction = new FuelTransaction
        {
            VehicleId = dto.VehicleId,
            FuelCardId = dto.FuelCardId,
            FuelTypeId = dto.FuelTypeId,
            PostedAt = dto.PostedAt,
            OdometerKm = dto.OdometerKm,
            Liters = dto.Liters,
            PricePerLiter = dto.PricePerLiter,
            EnergyKwh = dto.EnergyKwh,
            PricePerKwh = dto.PricePerKwh,
            TotalCost = dto.TotalCost,
            StationName = dto.StationName,
            ReceiptNumber = dto.ReceiptNumber,
            Notes = dto.Notes
        };

        var created = await _repo.CreateAsync(transaction);
        return MapToDto(created);
    }

    public async Task<bool> MarkSuspiciousAsync(int id, bool isSuspicious)
    {
        var result = await _repo.MarkSuspiciousAsync(id, isSuspicious);
        if (!result) throw new NotFoundException($"Fuel transaction with id {id} was not found.");
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Fuel transaction with id {id} was not found.");
        return true;
    }

    private static FuelTransactionDto MapToDto(FuelTransaction t) => new()
    {
        TransactionId = t.TransactionId,
        VehicleId = t.VehicleId,
        RegistrationNumber = t.Vehicle.RegistrationNumber,
        VehicleMake = t.Vehicle.Make?.Name,
        VehicleModel = t.Vehicle.Model?.Name,
        FuelCardId = t.FuelCardId,
        CardNumber = t.FuelCard?.CardNumber,
        FuelTypeId = t.FuelTypeId,
        FuelTypeName = t.FuelType.Label,
        PostedAt = t.PostedAt,
        OdometerKm = t.OdometerKm,
        Liters = t.Liters,
        PricePerLiter = t.PricePerLiter,
        EnergyKwh = t.EnergyKwh,
        PricePerKwh = t.PricePerKwh,
        TotalCost = t.TotalCost,
        StationName = t.StationName,
        ReceiptNumber = t.ReceiptNumber,
        IsSuspicious = t.IsSuspicious,
        Notes = t.Notes
    };
}
