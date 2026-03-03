namespace FleetManagement.Application.DTOs;

// -------------------------------------------------------
// Vendor DTOs
// -------------------------------------------------------
public class VendorDto
{
    public int VendorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}

public class CreateVendorDto
{
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

public class UpdateVendorDto
{
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool? IsActive { get; set; }
}

// -------------------------------------------------------
// MaintenanceType lookup DTO (for dropdowns)
// -------------------------------------------------------
public class MaintenanceTypeDto
{
    public int MaintenanceTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

// -------------------------------------------------------
// MaintenanceItem DTOs
// -------------------------------------------------------
public class MaintenanceItemDto
{
    public int ItemId { get; set; }
    public int OrderId { get; set; }
    public int MaintenanceTypeId { get; set; }
    public string MaintenanceTypeName { get; set; } = string.Empty;
    public decimal PartsCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal TotalCost => PartsCost + LaborCost;
    public string? Notes { get; set; }
}

public class CreateMaintenanceItemDto
{
    public int MaintenanceTypeId { get; set; }
    public decimal PartsCost { get; set; }
    public decimal LaborCost { get; set; }
    public string? Notes { get; set; }
}

// -------------------------------------------------------
// MaintenanceOrder DTOs
// -------------------------------------------------------
public class MaintenanceOrderDto
{
    public int OrderId { get; set; }
    public int VehicleId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public int? VendorId { get; set; }
    public string? VendorName { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ReportedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public int? OdometerKm { get; set; }
    public decimal? TotalCost { get; set; }
    public string? Description { get; set; }
    public string? CancelReason { get; set; }
    public List<MaintenanceItemDto> Items { get; set; } = new();
}

public class CreateMaintenanceOrderDto
{
    public int VehicleId { get; set; }
    public int VendorId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int OdometerKm { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class UpdateMaintenanceOrderDto
{
    public int? VendorId { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public string? Description { get; set; }
}

public class CloseMaintenanceOrderDto
{
    public int? OdometerKm { get; set; }
}

public class CancelMaintenanceOrderDto
{
    public string CancelReason { get; set; } = string.Empty;
}