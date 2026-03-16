namespace FleetManagement.Domain.Entities;

/// <summary>
/// Links a document to a vehicle with a document type classification.
/// Populated automatically when a document is uploaded for a vehicle-related entity
/// (Insurance, Registration, Inspection, etc.).
/// </summary>
public class VehicleDocument
{
    public int VehicleDocumentId { get; set; }
    public int VehicleId { get; set; }
    public int DocumentId { get; set; }

    /// <summary>
    /// 1 = Registration, 2 = Insurance, 3 = Inspection, 4 = Other
    /// </summary>
    public int DocumentTypeId { get; set; }

    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    // Navigation
    public Vehicle Vehicle { get; set; } = null!;
    public Document Document { get; set; } = null!;
}
