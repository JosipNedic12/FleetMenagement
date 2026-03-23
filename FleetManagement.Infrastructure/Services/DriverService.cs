using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class DriverService : IDriverService
{
    private readonly IDriverRepository _repo;

    public DriverService(IDriverRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<DriverDto>> GetAllAsync()
    {
        var drivers = await _repo.GetAllAsync();
        return drivers.Select(MapToDto);
    }

    public async Task<DriverDto?> GetByIdAsync(int id)
    {
        var driver = await _repo.GetByIdAsync(id);
        if (driver == null) throw new NotFoundException($"Driver with id {id} was not found.");
        return MapToDto(driver);
    }

    public async Task<DriverDto> CreateAsync(CreateDriverDto dto)
    {
        if (await _repo.LicenseNumberExistsAsync(dto.LicenseNumber))
            throw new ConflictException("A driver with this license number already exists.");

        var driver = new Driver
        {
            EmployeeId = dto.EmployeeId,
            LicenseNumber = dto.LicenseNumber.Trim(),
            LicenseExpiry = dto.LicenseExpiry,
            Notes = dto.Notes
        };

        var created = await _repo.CreateAsync(driver, dto.LicenseCategoryIds);
        return MapToDto(created);
    }

    public async Task<DriverDto?> UpdateAsync(int id, UpdateDriverDto dto)
    {
        var updated = await _repo.UpdateAsync(id, new Driver
        {
            LicenseNumber = dto.LicenseNumber ?? string.Empty,
            LicenseExpiry = dto.LicenseExpiry ?? default,
            Notes = dto.Notes
        }, dto.LicenseCategoryIds);

        if (updated == null) throw new NotFoundException($"Driver with id {id} was not found.");
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Driver with id {id} was not found.");
        return true;
    }

    private static DriverDto MapToDto(Driver d) => new()
    {
        DriverId = d.DriverId,
        EmployeeId = d.EmployeeId,
        FullName = $"{d.Employee.FirstName} {d.Employee.LastName}",
        Department = d.Employee.Department,
        LicenseNumber = d.LicenseNumber,
        LicenseExpiry = d.LicenseExpiry,
        LicenseCategories = d.LicenseCategories
                             .Select(lc => lc.LicenseCategory.Code)
                             .ToList(),
        Notes = d.Notes
    };
}
