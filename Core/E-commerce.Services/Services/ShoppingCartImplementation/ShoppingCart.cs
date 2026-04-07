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
            if (cartEntity == null) throw new ShoppingCartNotFoundException("Cart Not Found");
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
        
        public async Task<ShoppingCartDto> UpdateItemQuantityAsync(Guid cartId, int cartItemId, UpdateCartItemQuantityDto dto)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();
            var cartSpec = new GetShoppingCartSpec(cartId);
            var cart = await cartRepository.GetByIdWithSpecAsync(cartSpec); 

            if (cart == null)
                throw new ShoppingCartNotFoundException("Cart Not Found");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem == null)
                throw new NotFoundExceptionCustome("Cart Item Not Found");

            if (dto.Quantity <= 0)
                throw new BadRequestExceptionCustome("Quantity must be greater than zero");

            await ValidateProductStockAsync(cartItem.ProductVariantId, dto.Quantity);

            cartItem.Quantity = dto.Quantity;
            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastModifiedBy = cart.UserId;

            await _unitOfWork.SaveChangesAsync();

            return await GetCartAsync(cartId);
        }
      
        public async Task<ShoppingCartDto> RemoveItemFromCartAsync(Guid shoppingCartId, int cartItemId)
        {
            // 1 - check existance 
            var cartRepo = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();
            var cartSpec = new GetShoppingCartSpec(shoppingCartId);
            var cart = await cartRepo.GetByIdWithSpecAsync(cartSpec);

            if (cart == null) throw new ShoppingCartNotFoundException("Shopping Cart Not Found");
            
            // 2 - find the item in the cart
            var item = cart.CartItems.FirstOrDefault(c => c.Id == cartItemId);
            if (item == null) throw new CartItemNotFound($"Cart Item Not Found");

            // 3 - delete
            cart.CartItems.Remove(item);
            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastModifiedBy = cart.UserId;

            await _unitOfWork.SaveChangesAsync();
            return await GetCartAsync(shoppingCartId);
        }
       
        public async Task<ShoppingCartDto> MergeGuestCartToUserCartAsync(Guid guestCartId, string userId)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();

            // 1.get current User Cart
            var currentGuestCartSpec = new GetShoppingCartSpec(guestCartId);
            var currentGuestCart = await cartRepository.GetByIdWithSpecAsync(currentGuestCartSpec);

            // 2. check if currentGuestCart is empty or not found
            if (currentGuestCart == null || !currentGuestCart.CartItems.Any())
            {
                return await HandleEmptyGuestCartAsync(userId);
            }

            // 3. get the old cart if current is exist and has an items
            var oldUserCartSpec = new GetShoppingCartByUserIdSpec(userId);
            var oldUserCart = await cartRepository.GetByIdWithSpecAsync(oldUserCartSpec);

            // 4. if has no old user cart
            if (oldUserCart == null)
            {
                await AssignCartToUserAsync(currentGuestCart, userId);
                return await GetCartAsync(currentGuestCart.Id);
            }

            // 5. if has old cart and current cart so merging them
            MergeCartItemsWithStockValidation(currentGuestCart, oldUserCart, userId);

            // 6. مسح القديمة وتحديث الجديدة
            cartRepository.Delete(oldUserCart);
            currentGuestCart.UserId = userId;
            currentGuestCart.LastModifiedBy = userId;
            cartRepository.Update(currentGuestCart);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new BadRequestExceptionCustome("Error during merging carts");

            return await GetCartAsync(currentGuestCart.Id);
        }
       
        public async Task<bool> ClearCartAsync(Guid cartId)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();
            var cart = await cartRepository.GetByIdAsync(cartId);

            if (cart != null)
            {
                cartRepository.Delete(cart);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }

            return false;
        }

        #region Helper in Add item to cart
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

        #region Helper in Merging cart
        // merging the 2 carts
        private void MergeCartItemsWithStockValidation(ShoppingCartEntity currentGuestCart, ShoppingCartEntity oldUserCart, string userId)
        {
            foreach (var oldUserCartItem in oldUserCart.CartItems)
            {
                var existingUserItem = currentGuestCart.CartItems.FirstOrDefault(o => o.ProductVariantId == oldUserCartItem.ProductVariantId);

                if (existingUserItem != null)
                {
                    // total quantity
                    var requestedTotal = existingUserItem.Quantity + oldUserCartItem.Quantity;

                    // the maximum available quantity in DB
                    var availableStock = oldUserCartItem.ProductVariant.StockQuantity;

                    // to choice the requestedTotal if < availableStock and choice the availableStock if < requestedTotal
                    existingUserItem.Quantity = Math.Min(requestedTotal, availableStock);
                }
                else
                {
                    // if not exist item
                    currentGuestCart.CartItems.Add(new CartItemEntity
                    {
                        ProductVariantId = oldUserCartItem.ProductVariantId,
                        Quantity = oldUserCartItem.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        LastModifiedBy = userId,
                        CreatedBy = userId
                    });
                }
            }
        }    
        // hande the empty current user cart
        private async Task<ShoppingCartDto> HandleEmptyGuestCartAsync(string userId)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();

            var fallbackSpec = new GetShoppingCartByUserIdSpec(userId);
            var existingCart = await cartRepository.GetByIdWithSpecAsync(fallbackSpec);

            if (existingCart != null)
                return await GetCartAsync(existingCart.Id);

            throw new ShoppingCartNotFoundException("No Cart Found");
        }
        // assign the cart to the user and return it
        private async Task AssignCartToUserAsync(ShoppingCartEntity cart, string userId)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();

            cart.UserId = userId;
            cart.LastModifiedBy = userId;
            cart.UpdatedAt = DateTime.UtcNow;

            cartRepository.Update(cart);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
