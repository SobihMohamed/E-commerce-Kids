using E_commerce.Shared.Common.Pagination;
using E_commerce.Shared.Common.Params.Notification;
using E_commerce.Shared.Dto_s.Notificaiton;
using E_commerce.Shared.EnumsHelper.Notification;


namespace E_commerce.Abstraction.IService.Notification
{
    public interface INotificationService
    {
        Task SendNotificationAsync(NotificationContentDto notificationContentDto, params NotificationType[] types);

        // --- User/Client Operations (The Bell Icon) ---
        Task<PaginationResponse<NotificationDto>> GetUserNotificationsAsync(string userId, NotificationQueryParams queryParams);
        Task<bool> MarkAsReadAsync(Guid notificationId, string userId);
        Task<bool> MarkAllAsReadAsync(string userId);
    }
}
