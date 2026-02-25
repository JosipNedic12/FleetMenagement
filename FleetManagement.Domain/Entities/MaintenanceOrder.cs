namespace FleetManagement.Domain.Entities;

public class MaintenanceOrder
{
    public int OrderId { get; set; }
    public int VehicleId { get; set; }
    public int? VendorId { get; set; }

    // Status: open → in_progress → closed | open → cancelled
    public string Status { get; set; } = "open";

    public DateTime ReportedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public int? OdometerKm { get; set; }   // odometer at time of service
    public decimal? TotalCost { get; set; } // sum of all items, calculated on close
    public string? Description { get; set; }
    public string? CancelReason { get; set; }

    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation
    public Vehicle Vehicle { get; set; } = null!;
    public Vendor? Vendor { get; set; }
    public ICollection<MaintenanceItem> Items { get; set; } = new List<MaintenanceItem>();
}