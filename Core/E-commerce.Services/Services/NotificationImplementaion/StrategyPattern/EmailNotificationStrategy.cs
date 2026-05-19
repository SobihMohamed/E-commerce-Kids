using E_commerce.Abstraction.IService.Notification;
using E_commerce.Shared.Common.Dto.Notification.Settings;
using E_commerce.Shared.Dto_s.Notificaiton;
using E_commerce.Shared.EnumsHelper.Notification;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
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
        emailMessage.From.Add(new MailboxAddress("Mine Store", _emailSettings.Email));
        emailMessage.To.Add(new MailboxAddress("", ContentDto.Email));
        emailMessage.Subject = ContentDto.Subject;

        // 👇 1. التعديل هنا: استخدام BodyBuilder لعمل تصميم HTML احترافي يدعم العربي
        var builder = new BodyBuilder();
        var formattedBody = ContentDto.Body.Replace("\n", "<br>");

        builder.HtmlBody = $@"
            <div dir='rtl' style='font-family: Tahoma, Arial, sans-serif; font-size: 16px; color: #333333; line-height: 1.6; padding: 20px; background-color: #f9f9f9; border-radius: 8px;'>
                <div style='background-color: #ffffff; padding: 20px; border-radius: 8px; border: 1px solid #e0e0e0; box-shadow: 0 2px 4px rgba(0,0,0,0.05);'>
                    {formattedBody}
                </div>
                <p style='font-size: 12px; color: #888888; text-align: center; margin-top: 20px;'>
                    هذه رسالة تلقائية من نظام Mine Store، يرجى عدم الرد عليها.
                </p>
            </div>";

        emailMessage.Body = builder.ToMessageBody();

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