using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IVehicleAssignmentService
{
    Task<IEnumerable<VehicleAssignmentDto>> GetAllAsync(bool activeOnly = false);
    Task<VehicleAssignmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<VehicleAssignmentDto>> GetByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<VehicleAssignmentDto>> GetByDriverIdAsync(int driverId);
    Task<VehicleAssignmentDto> CreateAsync(CreateVehicleAssignmentDto dto);
    Task<VehicleAssignmentDto?> UpdateAsync(int id, UpdateVehicleAssignmentDto dto);
    Task<bool> EndAssignmentAsync(int id);
}
