using AutoMapper;
using E_commerce.Domain.Models.Address;
using E_commerce.Domain.Models.Order;
using E_commerce.Services.Resolver;
using E_commerce.Shared.Dto_s.Address;
using E_commerce.Shared.Dto_s.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper.OrderMapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressEntity, AddressDto>();

            CreateMap<OrderItemEntity, OrderItemDto>()
                .ForMember(dest => dest.CustomizedDesignUrl, opt =>
                    opt.MapFrom<PictureUrlResolver<OrderItemEntity, OrderItemDto>, string>(src => src.CustomizedDesignUrl!));

            CreateMap<OrderEntity, OrderDto>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderEntity, OrderSummaryDto>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()));
        }
    }
}
