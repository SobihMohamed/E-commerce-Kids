using AutoMapper;
using E_commerce.Abstraction.IService.ShoppingCart;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Domain.Models.Product;
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
        public async Task<ShoppingCartDto> AddItemToCartAsync(AddToCartDto dto, string? userId = null)
        {
            var variant = await ValidateProductStockAsync(dto.ProductVariantId, dto.Quantity);

            Guid targetCartId;

            if (dto.ShoppingCartId.HasValue)
            {
                targetCartId = await HandleExistingCartAsync(dto, variant, userId);
            }
            else
            {
                targetCartId = await CreateNewCartAsync(dto, userId);
            }

            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new BadRequestExceptionCustome("Error While adding to cart");

            return await GetCartAsync(targetCartId);
        }
        #region Private Helper Methods for AddItemToCartAsync
        private async Task<ProductVariantEntity> ValidateProductStockAsync(int variantId, int requestedQuantity)
        {
            var productVariantRepository = _unitOfWork.GetRepository<ProductVariantEntity, int>();
            var variant = await productVariantRepository.GetByIdAsync(variantId);

            if (variant == null)
                throw new NotFoundExceptionCustome("Product Not Available Now");

            if (variant.StockQuantity < requestedQuantity)
                throw new BadRequestExceptionCustome($"Quantity : {variant.StockQuantity} Not Available");

            return variant;
        }
        private async Task<Guid> HandleExistingCartAsync(AddToCartDto dto, ProductVariantEntity variant, string? userId)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();
            var cartSpec = new GetShoppingCartSpec(dto.ShoppingCartId!.Value);
            var cart = await cartRepository.GetByIdWithSpecAsync(cartSpec);

            if (cart == null)
                throw new ShoppingCartNotFoundException("Not Found Cart");

            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductVariantId == dto.ProductVariantId);

            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity + dto.Quantity > variant.StockQuantity)
                    throw new BadRequestExceptionCustome("Total Quantity Not Exist for this proudct");

                existingCartItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItemEntity
                {
                    ProductVariantId = dto.ProductVariantId,
                    Quantity = dto.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                });
            }

            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastModifiedBy = userId;

            return cart.Id;
        }
        private async Task<Guid> CreateNewCartAsync(AddToCartDto dto, string? userId)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();

            var newCart = new ShoppingCartEntity
            {
                Id = Guid.NewGuid(), // database will generate this, but we can set it here for clarity
                UserId = userId,
                CartItems = new List<CartItemEntity>
                {
                    new CartItemEntity
                    {
                        ProductVariantId = dto.ProductVariantId,
                        Quantity = dto.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                    }
                }
            };

            await cartRepository.AddAsync(newCart);

            return newCart.Id; 
        }
        #endregion

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
