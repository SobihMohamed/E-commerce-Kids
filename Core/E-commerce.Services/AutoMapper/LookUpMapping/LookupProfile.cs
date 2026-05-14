using AutoMapper;
using E_commerce.Domain.Models.Lookup;
using E_commerce.Shared.Dto_s.Lookups.Color;
using E_commerce.Shared.Dto_s.Lookups.Size;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper.LookUpMapping
{
    public class LookupProfile : Profile
    {
        public LookupProfile()
        {
            // Colors Mapping
            CreateMap<ColorEntity, ColorDto>().ReverseMap();
            CreateMap<ColorToCreateDto, ColorEntity>();

            // Sizes Mapping
            CreateMap<SizeEntity, SizeDto>().ReverseMap();
            CreateMap<SizeToCreateDto, SizeEntity>();
        }
    }
}
