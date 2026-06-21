using E_commerce.Abstraction.IService.Address;
using E_commerce.Abstraction.IService.Attachment;
using E_commerce.Abstraction.IService.Auth;
using E_commerce.Abstraction.IService.Category;
using E_commerce.Abstraction.IService.Dashboard;
using E_commerce.Abstraction.IService.Designs;
using E_commerce.Abstraction.IService.Lookup;
using E_commerce.Abstraction.IService.Notification;
using E_commerce.Abstraction.IService.Order;
using E_commerce.Abstraction.IService.Product;
using E_commerce.Abstraction.IService.Profile;
using E_commerce.Abstraction.IService.Shipping;
using E_commerce.Abstraction.IService.ShoppingCart;
using E_commerce.Abstraction.IService.Token;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.DbInitializer;
using E_commerce.Persistence.Implements.InitializerImplement;
using E_commerce.Persistence.ImplementsContracts.UowImmlementation;
using E_commerce.Services.Resolver;
using E_commerce.Services.Services;
using E_commerce.Services.Services.AddressImplementaion;
using E_commerce.Services.Services.AuthImplementation;
using E_commerce.Services.Services.CategoryImplemetation;
using E_commerce.Services.Services.DashboardImplementation;
using E_commerce.Services.Services.DesignImplementation;
using E_commerce.Services.Services.LookupImplementation;
using E_commerce.Services.Services.NotificationImplementaion;
using E_commerce.Services.Services.NotificationImplementation;
using E_commerce.Services.Services.NotificationImplementation.StrategyPattern;
using E_commerce.Services.Services.OrderImplementation;
using E_commerce.Services.Services.ProductImplementation;
using E_commerce.Services.Services.ProfileImplementaion;
using E_commerce.Services.Services.ShippingImplementation;
using E_commerce.Services.Services.ShoppingCartImplementation;
using E_commerce.Services.Services.TokenImplementation;
using E_commerce.Shared.Common.Dto.Notification.Settings;

namespace E_commerce.Web.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Settings
            // 1. Settings
            services.Configure<EmailSettingsDto>(configuration.GetSection("EmailSettings"));

            // 2. (Strategy Pattern)
            services.AddScoped<INotificationStrategy, EmailNotificationStrategy>();

            // 3. Services
            services.AddScoped<IUnitOfWork , UnitOfWork>();
            services.AddScoped<IDbInitializer , DbInitialized>();
            services.AddScoped<IAttachmentService, AttachementServices>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ILookupService, LookUpService>();
            services.AddScoped<IDesignsService, DesignsServices>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IShippingRateService, ShippingRateService>();

            services.AddTransient(typeof(PictureUrlResolver<,>));
            return services;
        }
    }
}
