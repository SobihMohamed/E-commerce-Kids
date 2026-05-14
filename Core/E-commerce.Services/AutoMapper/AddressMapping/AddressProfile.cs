using AutoMapper;
using E_commerce.Domain.Models.Address;
using E_commerce.Shared.Dto_s.Address;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper.AddressMapping
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressEntity, AddressDto>();
            CreateMap<AddressToSaveDto, AddressEntity>();
        }
    }
}
