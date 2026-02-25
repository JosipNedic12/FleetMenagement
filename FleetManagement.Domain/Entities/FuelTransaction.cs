namespace FleetManagement.Domain.Entities;

public class FuelTransaction
{
    public int TransactionId { get; set; }
    public int VehicleId { get; set; }
    public int? FuelCardId { get; set; }
    public int FuelTypeId { get; set; }
    public DateTime PostedAt { get; set; }
    public int? OdometerKm { get; set; }

    // Fuel fields (for non-electric)
    public decimal? Liters { get; set; }
    public decimal? PricePerLiter { get; set; }

    // Electric fields
    public decimal? EnergyKwh { get; set; }
    public decimal? PricePerKwh { get; set; }

    public decimal TotalCost { get; set; }
    public string? StationName { get; set; }
    public string? ReceiptNumber { get; set; }
    public bool IsSuspicious { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    // Navigation
    public Vehicle Vehicle { get; set; } = null!;
    public FuelCard? FuelCard { get; set; }
    public DcFuelType FuelType { get; set; } = null!;
}