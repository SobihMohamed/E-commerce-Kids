using AutoMapper;
using E_commerce.Domain.Models.Order;
using E_commerce.Shared.Dto_s.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper.DashboardMapping
{
    public class DashboardProfile : Profile
    {
        public DashboardProfile()
        {
            // 1. Mapping for Recent Orders in Dashboard
            CreateMap<OrderEntity, RecentOrderDto>()
                // ربط الـ Id
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src =>
                    src.User != null ? src.User.FullName : "Guest"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}