using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Entities;
using FleetManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FleetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private static readonly string[] AllowedEntityTypes =
        ["Vehicle", "Driver", "MaintenanceOrder", "Accident", "Inspection", "Insurance", "Registration", "Fine", "FuelTransaction"];

    // Maps entity type → document_type_id from fleet.dc_document_type
    // 5=INSURANCE, 6=REGISTRATION, 7=SERVICE, 8=OTHER
    private static readonly Dictionary<string, int> VehicleDocumentTypeMap = new()
    {
        ["Registration"] = 6,
        ["Insurance"]    = 5,
        ["Inspection"]   = 8,
    };

    private readonly FleetDbContext _db;
    private readonly IConfiguration _config;

    public DocumentsController(FleetDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // POST /api/documents
    [HttpPost]
    [Authorize(Roles = "Admin,FleetManager")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        [FromForm] string entityType,
        [FromForm] int entityId,
        IFormFile file,
        [FromForm] string? category = null,
        [FromForm] string? notes = null)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        if (!AllowedEntityTypes.Contains(entityType))
            return BadRequest($"entityType must be one of: {string.Join(", ", AllowedEntityTypes)}");

        var maxBytes = (_config.GetValue<int?>("FileStorage:MaxFileSizeMB") ?? 10) * 1024 * 1024;
        if (file.Length > maxBytes)
            return BadRequest($"File exceeds maximum size of {maxBytes / 1024 / 1024} MB.");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                         ?? User.FindFirst(JwtRegisteredClaimNames.Sub)
                         ?? User.FindFirst("sub");
        if (userIdClaim == null) return Unauthorized();
        var uploadedBy = int.Parse(userIdClaim.Value);

        var uploadRoot = _config["FileStorage:UploadPath"] ?? "uploads";
        var dir = Path.Combine(uploadRoot, entityType, entityId.ToString());
        Directory.CreateDirectory(dir);

        var safeFileName = Path.GetFileName(file.FileName);
        var storedName = $"{Guid.NewGuid():N}_{safeFileName}";
        var filePath = Path.Combine(dir, storedName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

        var doc = new Document
        {
            EntityType = entityType,
            EntityId = entityId,
            Category = category,
            FileName = safeFileName,
            ContentType = file.ContentType,
            FilePath = filePath,
            FileSize = file.Length,
            UploadedBy = uploadedBy,
            UploadedAt = DateTime.UtcNow,
            Notes = notes
        };

        _db.Documents.Add(doc);
        await _db.SaveChangesAsync();

        // Auto-link to vehicle_document for vehicle-related entity types
        if (VehicleDocumentTypeMap.TryGetValue(entityType, out var docTypeId))
        {
            var vehicleId = await ResolveVehicleIdAsync(entityType, entityId);
            if (vehicleId.HasValue)
            {
                _db.VehicleDocuments.Add(new VehicleDocument
                {
                    VehicleId      = vehicleId.Value,
                    DocumentId     = doc.DocumentId,
                    DocumentTypeId = docTypeId,
                    CreatedAt      = doc.UploadedAt,
                    CreatedBy      = null,
                });
                await _db.SaveChangesAsync();
            }
        }

        return CreatedAtAction(nameof(GetById), new { id = doc.DocumentId }, ToDto(doc));
    }

    // GET /api/documents?entityType=Vehicle&entityId=5
    [HttpGet]
    public async Task<IActionResult> GetByEntity([FromQuery] string entityType, [FromQuery] int entityId)
    {
        var docs = await _db.Documents
            .Where(d => d.EntityType == entityType && d.EntityId == entityId)
            .OrderByDescending(d => d.UploadedAt)
            .ToListAsync();

        return Ok(docs.Select(ToDto));
    }

    // GET /api/documents/{id}/download
    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var doc = await _db.Documents.FindAsync(id);
        if (doc == null) return NotFound();
        if (!System.IO.File.Exists(doc.FilePath)) return NotFound("File not found on disk.");

        return PhysicalFile(Path.GetFullPath(doc.FilePath), doc.ContentType, doc.FileName);
    }

    // GET /api/documents/{id} — used by CreatedAtAction
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doc = await _db.Documents.FindAsync(id);
        return doc == null ? NotFound() : Ok(ToDto(doc));
    }

    // DELETE /api/documents/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,FleetManager")]
    public async Task<IActionResult> Delete(int id)
    {
        var doc = await _db.Documents.FindAsync(id);
        if (doc == null) return NotFound();

        if (System.IO.File.Exists(doc.FilePath))
            System.IO.File.Delete(doc.FilePath);

        _db.Documents.Remove(doc);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // GET /api/documents/vehicle/{vehicleId}
    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> GetByVehicle(int vehicleId)
    {
        var items = await _db.VehicleDocuments
            .Where(vd => vd.VehicleId == vehicleId)
            .Include(vd => vd.Document)
            .OrderByDescending(vd => vd.CreatedAt)
            .ToListAsync();

        return Ok(items.Select(ToVehicleDocumentDto));
    }

    private async Task<int?> ResolveVehicleIdAsync(string entityType, int entityId)
    {
        return entityType switch
        {
            "Registration" => await _db.RegistrationRecords
                                    .Where(r => r.RegistrationId == entityId)
                                    .Select(r => (int?)r.VehicleId)
                                    .FirstOrDefaultAsync(),
            "Insurance"    => await _db.InsurancePolicies
                                    .Where(p => p.PolicyId == entityId)
                                    .Select(p => (int?)p.VehicleId)
                                    .FirstOrDefaultAsync(),
            "Inspection"   => await _db.Inspections
                                    .Where(i => i.InspectionId == entityId)
                                    .Select(i => (int?)i.VehicleId)
                                    .FirstOrDefaultAsync(),
            _              => null,
        };
    }

    private static VehicleDocumentDto ToVehicleDocumentDto(VehicleDocument vd) => new()
    {
        VehicleDocumentId = vd.VehicleDocumentId,
        VehicleId         = vd.VehicleId,
        DocumentId        = vd.DocumentId,
        DocumentTypeId    = vd.DocumentTypeId,
        DocumentTypeName  = vd.DocumentTypeId switch
        {
            5 => "Insurance",
            6 => "Registration",
            7 => "Service",
            8 => "Other",
            _ => "Other",
        },
        CreatedAt    = vd.CreatedAt,
        CreatedBy    = vd.CreatedBy,
        FileName     = vd.Document.FileName,
        ContentType  = vd.Document.ContentType,
        FileSize     = vd.Document.FileSize,
        UploadedAt   = vd.Document.UploadedAt,
        Notes        = vd.Document.Notes,
    };

    private static DocumentDto ToDto(Document d) => new()
    {
        DocumentId = d.DocumentId,
        EntityType = d.EntityType,
        EntityId = d.EntityId,
        Category = d.Category,
        FileName = d.FileName,
        ContentType = d.ContentType,
        FileSize = d.FileSize,
        UploadedBy = d.UploadedBy,
        UploadedAt = d.UploadedAt,
        Notes = d.Notes
    };
}
