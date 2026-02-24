namespace FleetManagement.Application.DTOs;

public class VehicleAssignmentDto
{
    public int AssignmentId { get; set; }
    public int VehicleId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public int DriverId { get; set; }
    public string DriverFullName { get; set; } = string.Empty;
    public string? Department { get; set; }
    public DateOnly AssignedFrom { get; set; }
    public DateOnly? AssignedTo { get; set; }
    public bool IsActive => AssignedTo == null || AssignedTo >= DateOnly.FromDateTime(DateTime.Today);
    public string? Notes { get; set; }
}

public class CreateVehicleAssignmentDto
{
    public int VehicleId { get; set; }
    public int DriverId { get; set; }
    public DateOnly AssignedFrom { get; set; }
    public DateOnly? AssignedTo { get; set; }
    public string? Notes { get; set; }
}

public class UpdateVehicleAssignmentDto
{
    public DateOnly? AssignedTo { get; set; }
    public string? Notes { get; set; }
}