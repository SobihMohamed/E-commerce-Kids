using AutoMapper;
using E_commerce.Domain.Models.Product;
using E_commerce.Services.Resolver;
using E_commerce.Shared.Dto_s.Product;
using E_commerce.Shared.Dto_s.Product.Variant;

namespace E_commerce.Services.AutoMapper.ProductMapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductVariantEntity, ProductVariantDto>()
                .ForMember(dest => dest.VariantId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
                .ForMember(dest => dest.ColorHexCode, opt => opt.MapFrom(src => src.Color.HexCode));
            
            // If we learn how to map collections of images,
            // we can use the same resolver for the product images as well.
            CreateMap<ProductImageEntity, ProductImageDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductImageEntity, ProductImageDto>, string>(src => src.ImageUrl));
             
            CreateMap<ProductEntity, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductEntity, ProductDto>, string>(src => src.MainImageUrl))
                .ForMember(dest => dest.AvailableColorHexCodes, opt => opt.MapFrom(src =>
                    src.Variants.Select(v => v.Color.HexCode).Distinct().ToList()));

            CreateMap<ProductEntity, ProductDetailsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductEntity, ProductDetailsDto>, string>(src => src.MainImageUrl))
                // he learn from the previous mapping how to map the collection of images and
                // use Url resolver for each image in the collection,
                // so we can just map the collection of images without needing to specify the resolver again.
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));
        }
    }
}