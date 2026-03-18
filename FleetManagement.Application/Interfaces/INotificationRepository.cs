using FleetManagement.Domain.Entities;

namespace FleetManagement.Application.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetByUserIdAsync(int userId, int limit = 50);
    Task<int> GetUnreadCountAsync(int userId);
    Task MarkAsReadAsync(int notificationId, int userId);
    Task MarkAllAsReadAsync(int userId);
    Task CreateManyAsync(IEnumerable<Notification> notifications);
    Task<bool> ExistsRecentAsync(string relatedEntityType, int relatedEntityId, string title, DateTime since);
}
