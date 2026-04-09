using E_commerce.Abstraction.IService.Auth;
using E_commerce.Abstraction.IService.Category;
using E_commerce.Abstraction.IService.Notification;
using E_commerce.Abstraction.IService.Order;
using E_commerce.Abstraction.IService.Product;
using E_commerce.Abstraction.IService.ShoppingCart;
using E_commerce.Abstraction.IService.Token;
using E_commerce.Services.Services.AuthImplementation;
using E_commerce.Services.Services.CategoryImplemetation;
using E_commerce.Services.Services.NotificationImplementation;
using E_commerce.Services.Services.OrderImplementation;
using E_commerce.Services.Services.ProductImplementation;
using E_commerce.Services.Services.ShoppingCartImplementation;
using E_commerce.Services.Services.TokenImplementation;
using E_commerce.Shared.Dto_s.Notificaiton.Settings;
using Microsoft.AspNetCore.Identity;

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

            // 3. Services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<INotificationService, NotificationService>();


            return services;
        }
    }
}
