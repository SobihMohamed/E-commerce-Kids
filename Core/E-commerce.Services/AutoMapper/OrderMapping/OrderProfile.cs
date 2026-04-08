using AutoMapper;
using E_commerce.Domain.Models.Order;
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
            CreateMap<OrderItemEntity, OrderItemDto>();

            CreateMap<OrderEntity, OrderDto>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}
