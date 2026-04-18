using E_commerce.Services.AutoMapper.AddressMapping;
using E_commerce.Services.AutoMapper.AuthMapping;
using E_commerce.Services.AutoMapper.CategroyMapping;
using E_commerce.Services.AutoMapper.LookUpMapping;
using E_commerce.Services.AutoMapper.OrderMapping;
using E_commerce.Services.AutoMapper.ProductMapping;
using E_commerce.Services.AutoMapper.ProfileMapping;
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
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new OrderProfile());
                cfg.AddProfile(new AddressProfile());   
                cfg.AddProfile(new LookupProfile());
            });
            return services;
        }
    }
}
