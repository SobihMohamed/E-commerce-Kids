using AutoMapper;
using E_commerce.Domain.Models.Category;
using E_commerce.Shared.Dto_s.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper.CategroyMapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryDto>()
                .ReverseMap();

            CreateMap<CategoryToCreateDto, CategoryEntity>();

            CreateMap<CategoryToUpdateDto, CategoryEntity>()
                            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
