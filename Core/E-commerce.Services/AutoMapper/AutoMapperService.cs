using E_commerce.Services.AutoMapper.AuthMapping;
using E_commerce.Services.AutoMapper.CategroyMapping;
using E_commerce.Services.AutoMapper.ProductMapping;
using E_commerce.Services.AutoMapper.ShoppingCartMapping;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper
{
    public static class AutoMapperService
    {
        public static IServiceCollection InjectAutoMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new AuthProfile());
                cfg.AddProfile(new CategoryProfile());
                cfg.AddProfile(new ProductProfile());
                cfg.AddProfile(new ShoppingCartProfile());
            });
            return services;
        }
    }
}
