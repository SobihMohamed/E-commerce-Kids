using AutoMapper;
using E_commerce.Domain.Models.Lookup;
using E_commerce.Domain.Models.Product;
using E_commerce.Services.Resolver;
using E_commerce.Shared.Dto_s.Lookups.Color;
using E_commerce.Shared.Dto_s.Lookups.Size;
using E_commerce.Shared.Dto_s.Product;
using E_commerce.Shared.Dto_s.Product.Variant;

namespace E_commerce.Services.AutoMapper.ProductMapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // 1. Mapping for Create Variant
            CreateMap<VariantToCreateDto, ProductVariantEntity>();

            CreateMap<ProductToCreateDto, ProductEntity>()
                .ForMember(dest => dest.MainImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());



            CreateMap<ProductVariantEntity, ProductVariantDto>()
                .ForMember(dest => dest.VariantId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.ColorHexCode, opt => opt.MapFrom(src => src.Color.HexCode));


            CreateMap<ProductEntity, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductEntity, ProductDto>, string>(src => src.MainImageUrl));

            CreateMap<ProductImageEntity, ProductImageDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductImageEntity, ProductImageDto>, string>(src => src.ImageUrl));


            // If we learn how to map collections of images,
            // we can use the same resolver for the product images as well.
            CreateMap<ProductEntity, ProductDetailsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductEntity, ProductDetailsDto>, string>(src => src.MainImageUrl))
                // he learn from the previous mapping how to map the collection of images and
                // use Url resolver for each image in the collection,
                // so we can just map the collection of images without needing to specify the resolver again.
    ;
            CreateMap<ProductToUpdateDto, ProductEntity>()
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<VariantToUpdateDto, ProductVariantEntity>()
                .ForMember(dest => dest.ColorId, opt => opt.Condition(src => src.ColorId > 0))
                .ForMember(dest => dest.SizeId, opt => opt.Condition(src => src.SizeId > 0));
        }
    }
}