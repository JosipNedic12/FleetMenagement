using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IAccidentService
{
    Task<IEnumerable<AccidentDto>> GetAllAsync();
    Task<AccidentDto?> GetByIdAsync(int id);
    Task<IEnumerable<AccidentDto>> GetByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<AccidentDto>> GetByDriverIdAsync(int driverId);
    Task<AccidentDto> CreateAsync(CreateAccidentDto dto);
    Task<AccidentDto?> UpdateAsync(int id, UpdateAccidentDto dto);
    Task<bool> DeleteAsync(int id);
}
