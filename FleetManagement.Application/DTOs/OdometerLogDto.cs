namespace FleetManagement.Application.DTOs;

public class OdometerLogDto
{
    public int LogId { get; set; }
    public int VehicleId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public int OdometerKm { get; set; }
    public DateOnly LogDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateOdometerLogDto
{
    public int VehicleId { get; set; }
    public int OdometerKm { get; set; }
    public DateOnly LogDate { get; set; }
    public string? Notes { get; set; }
}