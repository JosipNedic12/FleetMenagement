using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces;

public interface IMaintenanceOrderService
{
    Task<IEnumerable<MaintenanceOrderDto>> GetAllAsync();
    Task<MaintenanceOrderDto?> GetByIdAsync(int id);
    Task<IEnumerable<MaintenanceOrderDto>> GetByVehicleIdAsync(int vehicleId);
    Task<MaintenanceOrderDto> CreateAsync(CreateMaintenanceOrderDto dto);
    Task<MaintenanceOrderDto?> UpdateAsync(int id, UpdateMaintenanceOrderDto dto);
    Task<MaintenanceOrderDto?> StartAsync(int id);
    Task<MaintenanceOrderDto?> CloseAsync(int id, CloseMaintenanceOrderDto dto);
    Task<MaintenanceOrderDto?> CancelAsync(int id, CancelMaintenanceOrderDto dto);
    Task<MaintenanceItemDto> AddItemAsync(int orderId, CreateMaintenanceItemDto dto);
    Task<bool> DeleteItemAsync(int itemId);
}
