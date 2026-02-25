namespace FleetManagement.Domain.Entities;

public class FuelCard
{
    public int FuelCardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public int? AssignedVehicleId { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation
    public Vehicle? AssignedVehicle { get; set; }
}