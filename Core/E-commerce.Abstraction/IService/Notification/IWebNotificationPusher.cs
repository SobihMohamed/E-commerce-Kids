using E_commerce.Shared.Dto_s.Notificaiton;

namespace E_commerce.Abstraction.IServicesContract.Notification
{
    // This interface defines the contract for pushing web notifications to users.
    public interface IWebNotificationPusher
    {
        // this method is responsible for pushing a notification to a user asynchronously.
        Task PushToUserAsync(NotificationContentDto notificationContentDto);
    }
}
