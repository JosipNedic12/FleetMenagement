namespace FleetManagement.Application.DTOs;

// -------------------------------------------------------
// FuelCard DTOs
// -------------------------------------------------------
public class FuelCardDto
{
    public int FuelCardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public int? AssignedVehicleId { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? VehicleMake { get; set; }
    public string? VehicleModel { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
}

public class CreateFuelCardDto
{
    public string CardNumber { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public int? AssignedVehicleId { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public string? Notes { get; set; }
}

public class UpdateFuelCardDto
{
    public string? Provider { get; set; }
    public int? AssignedVehicleId { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public bool? IsActive { get; set; }
    public string? Notes { get; set; }
}

// -------------------------------------------------------
// FuelTransaction DTOs
// -------------------------------------------------------
public class FuelTransactionDto
{
    public int TransactionId { get; set; }
    public int VehicleId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public int? FuelCardId { get; set; }
    public string? CardNumber { get; set; }
    public int FuelTypeId { get; set; }
    public string FuelTypeName { get; set; } = string.Empty;
    public DateTime PostedAt { get; set; }
    public int? OdometerKm { get; set; }
    public decimal? Liters { get; set; }
    public decimal? PricePerLiter { get; set; }
    public decimal? EnergyKwh { get; set; }
    public decimal? PricePerKwh { get; set; }
    public decimal TotalCost { get; set; }
    public string? StationName { get; set; }
    public string? ReceiptNumber { get; set; }
    public bool IsSuspicious { get; set; }
    public string? Notes { get; set; }
}

public class CreateFuelTransactionDto
{
    public int VehicleId { get; set; }
    public int? FuelCardId { get; set; }
    public int FuelTypeId { get; set; }
    public DateTime PostedAt { get; set; }
    public int? OdometerKm { get; set; }

    // Fuel
    public decimal? Liters { get; set; }
    public decimal? PricePerLiter { get; set; }

    // Electric
    public decimal? EnergyKwh { get; set; }
    public decimal? PricePerKwh { get; set; }

    public decimal TotalCost { get; set; }
    public string? StationName { get; set; }
    public string? ReceiptNumber { get; set; }
    public string? Notes { get; set; }
}