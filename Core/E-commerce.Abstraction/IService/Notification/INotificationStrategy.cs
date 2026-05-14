using E_commerce.Shared.Dto_s.Notificaiton;
using E_commerce.Shared.EnumsHelper.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Notification
{
    public interface INotificationStrategy
    {
        NotificationType Type { get; }
        Task DeliverAsync(NotificationContentDto notificationContentDto);
    }
}
