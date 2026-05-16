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
        logger.LogInformation("📧 DeliverAsync started | To: {Email}", ContentDto.Email ?? "NULL");

        if (string.IsNullOrWhiteSpace(ContentDto.Email))
        {
            logger.LogWarning("❌ Email is empty, skipping.");
            return;
        }
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Kids-Ecommerce", _emailSettings.Email));
        emailMessage.To.Add(new MailboxAddress("", ContentDto.Email));
        emailMessage.Subject = ContentDto.Subject;
        emailMessage.Body = new TextPart("plain") { Text = ContentDto.Body };

        using var client = new SmtpClient();
        try
        {
            logger.LogInformation("🔌 Connecting to SMTP | Host: {Host} | Port: {Port}",
                _emailSettings.Host, _emailSettings.Port);

            await client.ConnectAsync(
                _emailSettings.Host,
                _emailSettings.Port,
                SecureSocketOptions.StartTls 
            );
            logger.LogInformation("🔐 Authenticating | User: {Email}", _emailSettings.Email);
            await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);

            await client.SendAsync(emailMessage);
            logger.LogInformation("✅ Email sent successfully to {Email}", ContentDto.Email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "⚠️ SMTP Failed | Host: {Host} | Port: {Port} | Error: {Message} | Inner: {Inner}",
                _emailSettings.Host, _emailSettings.Port, ex.Message, ex.InnerException?.Message ?? "none");
            throw;
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