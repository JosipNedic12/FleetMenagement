namespace FleetManagement.Domain.Entities;

public class MaintenanceItem
{
    public int ItemId { get; set; }
    public int OrderId { get; set; }
    public int MaintenanceTypeId { get; set; }
    public decimal PartsCost { get; set; }
    public decimal LaborCost { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    // Navigation
    public MaintenanceOrder Order { get; set; } = null!;
    public DcMaintenanceType MaintenanceType { get; set; } = null!;
}