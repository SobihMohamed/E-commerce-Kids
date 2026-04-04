using AutoMapper;
using E_commerce.Abstraction.IService.ShoppingCart;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Services.Specification.ShoppingCart;
using E_commerce.Shared.Dto_s.ShoppingCart.RequestDto;
using E_commerce.Shared.Dto_s.ShoppingCart.ResponseDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.ShoppingCartImplementation
{
    public class ShoppingCart(IUnitOfWork _unitOfWork, IMapper _mapper) : IShoppingCartService
    {
        public async Task<ShoppingCartDto> GetCartAsync(Guid cartId)
        {
            // Get Cart Repository
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();
            //specifications for fetching the cart with its items and related product details 
            var cartSpecification = new GetShoppingCartSpec(cartId);
            // Fetch the cart from the database
            var cartEntity = await cartRepository.GetByIdWithSpecAsync(cartSpecification);
            // check if cart exists
            if (cartEntity == null) throw new ShoppingCartNotFoundException();
            // Map the cart entity to a DTO
            var cartDto = _mapper.Map<ShoppingCartDto>(cartEntity);
            // Return the cart DTO
            return cartDto;
        }
        public Task<ShoppingCartDto> AddItemToCartAsync(AddToCartDto dto, string? userId = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ClearCartAsync(Guid cartId)
        {
            throw new NotImplementedException();
        }


        public Task<ShoppingCartDto> MergeGuestCartToUserCartAsync(Guid guestCartId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCartDto> RemoveItemFromCartAsync(Guid cartId, int cartItemId)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCartDto> UpdateItemQuantityAsync(Guid cartId, int cartItemId, UpdateCartItemQuantityDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
