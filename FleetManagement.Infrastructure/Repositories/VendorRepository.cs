using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;
using FleetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure.Repositories;

public class VendorRepository : IVendorRepository
{
    private readonly FleetDbContext _context;
    public VendorRepository(FleetDbContext context) => _context = context;

    public async Task<IEnumerable<Vendor>> GetAllAsync() =>
        await _context.Vendors
            .Where(v => !v.IsDeleted)
            .OrderBy(v => v.Name)
            .ToListAsync();

    public async Task<Vendor?> GetByIdAsync(int id) =>
        await _context.Vendors
            .FirstOrDefaultAsync(v => v.VendorId == id && !v.IsDeleted);

    public async Task<Vendor> CreateAsync(Vendor vendor)
    {
        vendor.CreatedAt = DateTime.UtcNow;
        _context.Vendors.Add(vendor);
        await _context.SaveChangesAsync();
        return vendor;
    }

    public async Task<Vendor?> UpdateAsync(int id, Vendor updated)
    {
        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor == null || vendor.IsDeleted) return null;

        vendor.ContactPerson = updated.ContactPerson;
        vendor.Phone = updated.Phone;
        vendor.Email = updated.Email;
        vendor.Address = updated.Address;
        vendor.IsActive = updated.IsActive;
        vendor.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return vendor;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor == null || vendor.IsDeleted) return false;

        vendor.IsDeleted = true;
        vendor.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}