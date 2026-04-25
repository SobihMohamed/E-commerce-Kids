using AutoMapper;
using E_commerce.Domain.Models.Category;
using E_commerce.Services.Resolver;
using E_commerce.Shared.Dto_s.Category;

namespace E_commerce.Services.AutoMapper.CategroyMapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // 1. Entity to DTO (Return Data)
            CreateMap<CategoryEntity, CategoryDto>()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver<CategoryEntity, CategoryDto>, string>(src => src.PictureUrl));
            ;

            // 2. CreateDto to Entity
            CreateMap<CategoryToCreateDto, CategoryEntity>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());

            // 3. UpdateDto to Entity
            CreateMap<CategoryToUpdateDto, CategoryEntity>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore())
                // ignore null values during mapping to avoid overwriting existing values with null
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}