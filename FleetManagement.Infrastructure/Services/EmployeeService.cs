using FleetManagement.Application.DTOs;
using FleetManagement.Application.Exceptions;
using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Infrastructure.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;

    public EmployeeService(IEmployeeRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _repo.GetAllAsync();
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _repo.GetByIdAsync(id);
        if (employee == null) throw new NotFoundException($"Employee with id {id} was not found.");
        return MapToDto(employee);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        if (await _repo.EmailExistsAsync(dto.Email))
            throw new ConflictException("An employee with this email already exists.");

        var employee = new Employee
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Department = dto.Department,
            Email = dto.Email.ToLower().Trim(),
            Phone = dto.Phone
        };

        var created = await _repo.CreateAsync(employee);
        return MapToDto(created);
    }

    public async Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var updated = await _repo.UpdateAsync(id, new Employee
        {
            Department = dto.Department,
            Phone = dto.Phone,
            IsActive = dto.IsActive ?? true
        });

        if (updated == null) throw new NotFoundException($"Employee with id {id} was not found.");
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repo.DeleteAsync(id);
        if (!result) throw new NotFoundException($"Employee with id {id} was not found.");
        return true;
    }

    private static EmployeeDto MapToDto(Employee e) => new()
    {
        EmployeeId = e.EmployeeId,
        FirstName = e.FirstName,
        LastName = e.LastName,
        Department = e.Department,
        Email = e.Email,
        Phone = e.Phone,
        IsActive = e.IsActive,
        HasDriverProfile = e.Driver != null && !e.Driver.IsDeleted,
        HasAppUser = e.AppUser != null && e.AppUser.IsActive
    };
}
