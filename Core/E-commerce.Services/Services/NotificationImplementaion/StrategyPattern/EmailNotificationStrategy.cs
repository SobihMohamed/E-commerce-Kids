using E_commerce.Abstraction.IService.Notification;
using E_commerce.Shared.Common.Dto.Notification.Settings;
using E_commerce.Shared.Dto_s.Notificaiton;
using E_commerce.Shared.EnumsHelper.Notification;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging; // 👈 تأكد من وجود ده
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

public class EmailNotificationStrategy(
    IOptions<EmailSettingsDto> emailSettings,
    ILogger<EmailNotificationStrategy> logger) : INotificationStrategy 
{
    private readonly EmailSettingsDto _emailSettings = emailSettings.Value;
    public NotificationType Type => NotificationType.Email;

    public async Task DeliverAsync(NotificationContentDto ContentDto)
    {
        if (string.IsNullOrWhiteSpace(ContentDto.Email))
            return;

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Kids-Ecommerce", _emailSettings.Email));
        emailMessage.To.Add(new MailboxAddress("", ContentDto.Email));
        emailMessage.Subject = ContentDto.Subject;
        emailMessage.Body = new TextPart("plain") { Text = ContentDto.Body };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.Auto);
            await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
            await client.SendAsync(emailMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "⚠️ فشل إرسال البريد الإلكتروني إلى {Email}. السبب: {Message}", ContentDto.Email, ex.Message);

            return;
        }
        finally
        {
            if (client.IsConnected)
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}