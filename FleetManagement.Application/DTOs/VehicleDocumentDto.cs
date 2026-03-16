namespace FleetManagement.Application.DTOs;

public class VehicleDocumentDto
{
    public int VehicleDocumentId { get; set; }
    public int VehicleId { get; set; }
    public int DocumentId { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentTypeName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    // Flattened document info
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public string? Notes { get; set; }
}
