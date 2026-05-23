using E_commerce.Domain.Models.User;
using E_commerce.Persistence.E_commerceDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;

namespace E_commerce.Web.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection InjectIdentityCore(this IServiceCollection services)
        {
            // Add Identity services
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                // for forget password 
                // to van used => var otp = await _userManager.GeneratePasswordResetTokenAsync(user);
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            })
                 .AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<EcommerceDbContext>()
                 .AddDefaultTokenProviders();
            return services;
        }
        public static IServiceCollection InjectRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("OtpPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(2); // time to expire is 2 minutes 
                    opt.PermitLimit = 3; // numbers to try is 3 times
                    opt.QueueLimit = 0; // no queue, requests will be rejected immediately when the limit is reached
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            return services;
        }
    }
}
