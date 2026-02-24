namespace FleetManagement.Domain.Entities;

public class VehicleAssignment
{
    public int AssignmentId { get; set; }
    public int VehicleId { get; set; }
    public int DriverId { get; set; }
    public DateOnly AssignedFrom { get; set; }
    public DateOnly? AssignedTo { get; set; }        // null = currently active
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation
    public Vehicle Vehicle { get; set; } = null!;
    public Driver Driver { get; set; } = null!;
}