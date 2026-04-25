using E_commerce.Abstraction.IService.Notification;
using E_commerce.Shared.Common.Pagination;
using E_commerce.Shared.Common.Params.Notification;
using E_commerce.Shared.Dto_s.Notificaiton;
using E_commerce.Shared.EnumsHelper.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.NotificationImplementation
{
    public class NotificationService(IEnumerable<INotificationStrategy> _notificationStrategies) : INotificationService
    {
        public Task<PaginationResponse<NotificationDto>> GetUserNotificationsAsync(string userId, NotificationQueryParams queryParams)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkAllAsReadAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkAsReadAsync(Guid notificationId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task SendNotificationAsync(NotificationContentDto message, NotificationType type)
        {
            var strategy = _notificationStrategies.FirstOrDefault(s => s.Type == type);

            await strategy!.DeliverAsync(message);
        }

        public Task SendNotificationAsync(NotificationContentDto notificationContentDto, params NotificationType[] types)
        {
            throw new NotImplementedException();
        }
    }
}
