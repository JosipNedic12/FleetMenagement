using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IVendorService
{
    Task<IEnumerable<VendorDto>> GetAllAsync();
    Task<VendorDto?> GetByIdAsync(int id);
    Task<VendorDto> CreateAsync(CreateVendorDto dto);
    Task<VendorDto?> UpdateAsync(int id, UpdateVendorDto dto);
    Task<bool> DeleteAsync(int id);
}
