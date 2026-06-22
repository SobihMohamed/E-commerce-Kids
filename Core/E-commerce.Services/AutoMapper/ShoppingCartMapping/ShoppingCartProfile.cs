using AutoMapper;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Services.Resolver;
using E_commerce.Shared.Dto_s.ShoppingCart.RequestDto;
using E_commerce.Shared.Dto_s.ShoppingCart.ResponseDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper.ShoppingCartMapping
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {

            CreateMap<ShoppingCartEntity, ShoppingCartDto>()
                .ForMember(dest => dest.CartItems,
                           opt => opt.MapFrom(src => src.CartItems.OrderByDescending(i => i.Id)));

            CreateMap<AddToCartDto,CartItemEntity>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProductVariantId, opt => opt.MapFrom(src => src.ProductVariantId))
                .ForMember(dest => dest.ShoppingCartId, opt => opt.MapFrom(src => src.ShoppingCartId ?? Guid.Empty))
                .ForMember(dest => dest.DesignId, opt => opt.MapFrom(src => src.DesignId));

            CreateMap<CartItemEntity, CartItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductVariant.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductVariant.Product.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.ProductVariant.Product.Price))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.ProductVariant.Color.Name))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.ProductVariant.Size.Name))
                .ForMember(dest => dest.AvailableStock, opt => opt.MapFrom(src => src.ProductVariant.StockQuantity))
                // refactor this to use the resolver instead of the map from because we need to get the main image url
                // from the product and we need to use the resolver to get it because we need to check
                // if the product has a main image or not if it has a main image we return it if not we return null
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom<PictureUrlResolver<CartItemEntity, CartItemDto>, string>(src =>
                    !string.IsNullOrEmpty(src.CustomizedPreviewUrl)
                    ? src.CustomizedPreviewUrl               // لو في بريفيو مخصص استخدمه
                    : src.ProductVariant.Product.MainImageUrl // لو مفيش استخدم صورة المنتج الأصلية
                ))


                // NEW: Customization Details Mapping

                .ForMember(dest => dest.DesignId, opt => opt.MapFrom(src => src.DesignId))

                // get the design name if the design is not null otherwise return null
                .ForMember(dest => dest.DesignName, opt => opt.MapFrom(src => src.Design != null ? src.Design.Name : null))

                // return the design image url if the design is not null otherwise return empty string
                .ForMember(dest => dest.DesignImageUrl, opt => opt.MapFrom<PictureUrlResolver<CartItemEntity, CartItemDto>, string>
                    (src => src.Design != null ? src.Design.ImageUrl : string.Empty));

                // calculate the design price based on whether the design is null or not (if the design is null then the price is 0 otherwise the price is 15)
                //.ForMember(dest => dest.DesignPrice, opt => opt.MapFrom(src => src.DesignId.HasValue ? 50m : 0m));
        }
    }
}
