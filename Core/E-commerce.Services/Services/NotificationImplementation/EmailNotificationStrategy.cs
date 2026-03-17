using E_commerce.Abstraction.IService.Notification;
using E_commerce.Shared.Dto_s.Notificaiton;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;
using E_commerce.Shared.Dto_s.Notificaiton.Settings;

namespace E_commerce.Services.Services.NotificationImplementation
{
    public class EmailNotificationStrategy(IOptions<EmailSettings> emailSettings) : INotificationStrategy
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;
        public NotificationType Type => NotificationType.Email;

        public async Task SendAsync(MessageDto message)
        {
            // 1. prepare message
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("E-Commerce Kids App", _emailSettings.Email));
            emailMessage.To.Add(new MailboxAddress("", message.To)); // the reciver 
            emailMessage.Subject = message.Subject;

            // 2. for design body
            emailMessage.Body = new TextPart("plain")
            {
                Text = message.Body
            };

            // 3.send emails
            using var client = new SmtpClient();
            try
            {
                // Connect to the SMTP server
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);

                // Authenticate with the email server
                await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);

                // Send the email
                await client.SendAsync(emailMessage);
            }
            finally
            {
                // close connections
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
