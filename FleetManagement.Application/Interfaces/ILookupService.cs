using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface ILookupService
{
    Task<IEnumerable<MakeDto>> GetMakesAsync();
    Task<IEnumerable<ModelDto>> GetModelsByMakeAsync(int makeId);
    Task<IEnumerable<VehicleCategoryDto>> GetVehicleCategoriesAsync();
    Task<IEnumerable<FuelTypeDto>> GetFuelTypesAsync();
    Task<IEnumerable<LicenseCategoryDto>> GetLicenseCategoriesAsync();
    Task<IEnumerable<MaintenanceTypeDto>> GetMaintenanceTypesAsync();
}
