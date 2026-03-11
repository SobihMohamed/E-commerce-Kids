using E_commerce.Domain.Models.User;
using E_commerce.Persistence.E_commerceDbContext;
using Microsoft.AspNetCore.Identity;

namespace E_commerce.Web.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection InjectIdentityCore(this IServiceCollection services)
        {
            // Add Identity services
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
                 .AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<EcommerceDbContext>()
                 .AddDefaultTokenProviders();
            return services;
        }
    }
}
