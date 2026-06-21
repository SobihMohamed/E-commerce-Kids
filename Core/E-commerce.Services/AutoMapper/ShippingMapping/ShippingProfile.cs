using AutoMapper;
using E_commerce.Domain.Models.Shipping;
using E_commerce.Shared.Dto_s.Shipping;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper.ShippingMapping
{
    public class ShippingProfile : Profile
    {
        public ShippingProfile()
        {
            CreateMap<ShippingRates, ShippingRateDto>();
        }
    }
}
