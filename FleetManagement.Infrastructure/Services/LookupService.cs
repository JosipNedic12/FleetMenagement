using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces;
using FleetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure.Services;

public class LookupService : ILookupService
{
    private readonly FleetDbContext _context;

    public LookupService(FleetDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MakeDto>> GetMakesAsync()
    {
        return await _context.VehicleMakes
            .Where(m => m.IsActive)
            .OrderBy(m => m.Name)
            .Select(m => new MakeDto { MakeId = m.MakeId, Name = m.Name })
            .ToListAsync();
    }

    public async Task<IEnumerable<ModelDto>> GetModelsByMakeAsync(int makeId)
    {
        return await _context.VehicleModels
            .Where(m => m.MakeId == makeId && m.IsActive)
            .OrderBy(m => m.Name)
            .Select(m => new ModelDto { ModelId = m.ModelId, MakeId = m.MakeId, Name = m.Name })
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleCategoryDto>> GetVehicleCategoriesAsync()
    {
        return await _context.VehicleCategories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new VehicleCategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<FuelTypeDto>> GetFuelTypesAsync()
    {
        return await _context.FuelTypes
            .Where(f => f.IsActive)
            .OrderBy(f => f.Label)
            .Select(f => new FuelTypeDto
            {
                FuelTypeId = f.FuelTypeId,
                Code = f.Code,
                Label = f.Label,
                IsElectric = f.IsElectric
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<LicenseCategoryDto>> GetLicenseCategoriesAsync()
    {
        return await _context.LicenseCategories
            .Where(lc => lc.IsActive)
            .OrderBy(lc => lc.Code)
            .Select(lc => new LicenseCategoryDto
            {
                LicenseCategoryId = lc.LicenseCategoryId,
                Code = lc.Code,
                Description = lc.Description
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<MaintenanceTypeDto>> GetMaintenanceTypesAsync()
    {
        return await _context.MaintenanceTypes
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .Select(t => new MaintenanceTypeDto
            {
                MaintenanceTypeId = t.MaintenanceTypeId,
                Name = t.Name,
                Description = t.Description
            })
            .ToListAsync();
    }
}
