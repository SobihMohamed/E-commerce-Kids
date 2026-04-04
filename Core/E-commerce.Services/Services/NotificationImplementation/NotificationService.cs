using E_commerce.Abstraction.IService.Notification;
using E_commerce.Shared.Dto_s.Notificaiton;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.NotificationImplementation
{
    public class NotificationService(IEnumerable<INotificationStrategy> _notificationStrategies) : INotificationService
    {
        public async Task SendNotificationAsync(MessageDto message, NotificationType type)
        {
            var strategy = _notificationStrategies.FirstOrDefault(s => s.Type == type);

            await strategy!.DeliverAsync(message);
        }
    }
}
