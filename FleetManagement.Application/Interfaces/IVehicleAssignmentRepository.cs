using FleetManagement.Domain.Entities;

namespace FleetManagement.Application.Interfaces;

public interface IVehicleAssignmentRepository
{
    Task<IEnumerable<VehicleAssignment>> GetAllAsync(bool activeOnly = false);
    Task<VehicleAssignment?> GetByIdAsync(int id);
    Task<IEnumerable<VehicleAssignment>> GetByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<VehicleAssignment>> GetByDriverIdAsync(int driverId);
    Task<VehicleAssignment?> GetActiveByVehicleIdAsync(int vehicleId);
    Task<bool> VehicleHasActiveAssignmentAsync(int vehicleId);
    Task<VehicleAssignment> CreateAsync(VehicleAssignment assignment);
    Task<VehicleAssignment?> UpdateAsync(int id, UpdateAssignmentData data);
    Task<bool> EndAssignmentAsync(int id);
}

public record UpdateAssignmentData(
    DateOnly? AssignedTo,
    string? Notes
);