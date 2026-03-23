using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class VendorService : IVendorService
{
    private readonly IVendorRepository _repo;

    public VendorService(IVendorRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<VendorDto>> GetAllAsync()
    {
        var vendors = await _repo.GetAllAsync();
        return vendors.Select(MapToDto);
    }

    public async Task<VendorDto?> GetByIdAsync(int id)
    {
        var vendor = await _repo.GetByIdAsync(id);
        if (vendor == null) throw new NotFoundException($"Vendor with id {id} was not found.");
        return MapToDto(vendor);
    }

    public async Task<VendorDto> CreateAsync(CreateVendorDto dto)
    {
        var vendor = new Vendor
        {
            Name = dto.Name.Trim(),
            ContactPerson = dto.ContactPerson,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            IsActive = true
        };

        var created = await _repo.CreateAsync(vendor);
        return MapToDto(created);
    }

    public async Task<VendorDto?> UpdateAsync(int id, UpdateVendorDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new NotFoundException($"Vendor with id {id} was not found.");

        var updated = await _repo.UpdateAsync(id, new Vendor
        {
            ContactPerson = dto.ContactPerson ?? existing.ContactPerson,
            Phone = dto.Phone ?? existing.Phone,
            Email = dto.Email ?? existing.Email,
            Address = dto.Address ?? existing.Address,
            IsActive = dto.IsActive ?? existing.IsActive
        });

        if (updated == null) throw new NotFoundException($"Vendor with id {id} was not found.");
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Vendor with id {id} was not found.");
        return true;
    }

    private static VendorDto MapToDto(Vendor v) => new()
    {
        VendorId = v.VendorId,
        Name = v.Name,
        ContactPerson = v.ContactPerson,
        Phone = v.Phone,
        Email = v.Email,
        Address = v.Address,
        IsActive = v.IsActive
    };
}
