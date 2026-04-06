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
                .ForMember(dest => dest.ShoppingCartId, opt => opt.MapFrom(src => src.ShoppingCartId ?? Guid.Empty));


            CreateMap<CartItemEntity, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductVariant.Product.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.ProductVariant.Product.Price))
                // refactor this to use the resolver instead of the map from because we need to get the main image url
                // from the product and we need to use the resolver to get it because we need to check
                // if the product has a main image or not if it has a main image we return it if not we return null
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom<PictureUrlResolver<CartItemEntity, CartItemDto>, string>
                    (src => src.ProductVariant.Product.MainImageUrl))

                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.ProductVariant.Color.Name))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.ProductVariant.Size.Name))

                .ForMember(dest => dest.AvailableStock, opt => opt.MapFrom(src => src.ProductVariant.StockQuantity));
        }
    }
}
