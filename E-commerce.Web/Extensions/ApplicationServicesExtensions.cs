using E_commerce.Abstraction.IService.Auth;
using E_commerce.Abstraction.IService.Notification;
using E_commerce.Abstraction.IService.Token;
using E_commerce.Services.Services.AuthImplementation;
using E_commerce.Services.Services.NotificationImplementation;
using E_commerce.Services.Services.TokenImplementation;
using E_commerce.Shared.Dto_s.Notificaiton.Settings;

namespace E_commerce.Web.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Settings
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            // 2. (Strategy Pattern)
            services.AddScoped<INotificationStrategy, EmailNotificationStrategy>();
            //services.AddScoped<INotificationStrategy, WhatsNotificationStrategy>();
            //services.AddScoped<INotificationStrategy, SmsNotificationStrategy>();

            // 3. Services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INotificationService, NotificationService>();


            return services;
        }
    }
}
