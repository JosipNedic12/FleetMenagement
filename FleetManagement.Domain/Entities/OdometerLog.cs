namespace FleetManagement.Domain.Entities;

public class OdometerLog
{
    public int LogId { get; set; }
    public int VehicleId { get; set; }
    public int OdometerKm { get; set; }
    public DateOnly LogDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    // Navigation
    public Vehicle Vehicle { get; set; } = null!;
}