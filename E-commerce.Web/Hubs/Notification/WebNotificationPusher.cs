using Microsoft.AspNetCore.SignalR;
using E_commerce.Abstraction.IServicesContract.Notification;
using E_commerce.Shared.Dto_s.Notificaiton;
using E_commerce.Web.Hubs.Notification;


namespace E_commerce.Services.Services.NotificationImplementation
{
    // why NotificationHub is used in the constructor?
    // because we need to send notifications to users in real-time using SignalR,
    // and the IHubContext<NotificationHub> allows us to access the SignalR hub context to send messages to connected clients.

    // even if the NotificationHub has no methods defined,
    // it serves as a central point for managing real-time communication between the server and clients in one way.
    public class WebNotificationPusher(IHubContext<NotificationHub> notificationHub) : IWebNotificationPusher
    {
        public async Task PushToUserAsync(NotificationContentDto notificationContentDto)
        {
            // SendAsync method is used to send a message to a specific user identified by notificationContentDto.To.
            await notificationHub.Clients.User(notificationContentDto.UserId)
                // ReceiveNotification is the name of the client-side method that will be invoked when the notification is received.
                // used in frontend to handle the incoming notification and display it to the user.
                .SendAsync("ReceiveNotification", new
                {
                    subject = notificationContentDto.Subject,
                    body = notificationContentDto.Body,
                    referenceId = notificationContentDto.ReferenceId,
                    date = DateTime.UtcNow
                });
        }
    }
}
