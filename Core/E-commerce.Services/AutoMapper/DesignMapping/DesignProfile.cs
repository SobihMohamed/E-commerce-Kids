using AutoMapper;
using E_commerce.Domain.Models.Designs;
using E_commerce.Services.Resolver;
using E_commerce.Shared.Dto_s.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper.DesignMapping
{
    public class DesignProfile : Profile
    {
        public DesignProfile()
        {
            // 1. Get (Entity to Dto) - Using the Resolver for the ImageUrl
            CreateMap<DesignsEntity, DesignDto>()
                .ForMember(dest => dest.ImageUrl, opt =>
                    opt.MapFrom<PictureUrlResolver<DesignsEntity, DesignDto>, string>(src => src.ImageUrl));

            // 2. Create (Dto to Entity) - Ignore ImageUrl as we set it manually in the Service
            CreateMap<DesignToCreateDto, DesignsEntity>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }
    }
}
