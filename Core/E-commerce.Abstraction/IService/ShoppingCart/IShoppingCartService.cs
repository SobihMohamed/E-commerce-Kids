using E_commerce.Shared.Dto_s.ShoppingCart.RequestDto;
using E_commerce.Shared.Dto_s.ShoppingCart.ResponseDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.ShoppingCart
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDto> GetCartAsync(Guid cartId);

        Task<ShoppingCartDto> AddItemToCartAsync(AddToCartDto dto, string? userId = null);

        Task<ShoppingCartDto> UpdateItemQuantityAsync(Guid cartId, int cartItemId, UpdateCartItemQuantityDto dto);

        Task<ShoppingCartDto> RemoveItemFromCartAsync(Guid cartId, int cartItemId);

        Task<bool> ClearCartAsync(Guid cartId);

        Task<ShoppingCartDto> MergeGuestCartToUserCartAsync(Guid guestCartId, string userId);
    }
}
