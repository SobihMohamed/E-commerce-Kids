using AutoMapper;
using E_commerce.Abstraction.IService.Attachment;
using E_commerce.Abstraction.IService.ShoppingCart;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Domain.Models.Product;
using E_commerce.Services.Specification.ShoppingCart;
using E_commerce.Shared.Dto_s.ShoppingCart.RequestDto;
using E_commerce.Shared.Dto_s.ShoppingCart.ResponseDto;

namespace E_commerce.Services.Services.ShoppingCartImplementation
{
    public class ShoppingCartService(IUnitOfWork _unitOfWork, IMapper _mapper , IAttachmentService attachmentService) : IShoppingCartService
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

            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();
            ShoppingCartEntity? existingCart = null;

            if (!string.IsNullOrEmpty(userId))
            {
                var cartSpec = new GetShoppingCartByUserIdSpec(userId);
                existingCart = await cartRepository.GetByIdWithSpecAsync(cartSpec);
            }
            else if (dto.ShoppingCartId.HasValue)
            {
                var cartSpec = new GetShoppingCartSpec(dto.ShoppingCartId.Value);
                existingCart = await cartRepository.GetByIdWithSpecAsync(cartSpec);
            }

            Guid targetCartId = existingCart != null ? existingCart.Id : Guid.NewGuid();

            CartItemEntity? existingCartItem = null;

            if (existingCart != null)
            {
                existingCartItem = existingCart.CartItems.FirstOrDefault(i =>
                    i.ProductVariantId == dto.ProductVariantId &&
                    i.DesignId == dto.DesignId);

                if (existingCartItem != null)
                {
                    int totalRequestedQuantity = existingCartItem.Quantity + dto.Quantity;
                    if (variant.StockQuantity < totalRequestedQuantity)
                    {
                        throw new BadRequestExceptionCustome(
                            $"الكمية الإجمالية المطلوبة ({totalRequestedQuantity}) تتعدى المخزن المتاح ({variant.StockQuantity})");
                    }
                }
            }

            string? uploadedImageUrl = null;

            if (existingCart != null && existingCartItem != null)
            {
                existingCartItem.Quantity += dto.Quantity;
                existingCart.UpdatedAt = DateTime.UtcNow;
                existingCart.LastModifiedBy = userId;
                //existingCartItem.ProductVariant = null;

                //var cartItemRepo = _unitOfWork.GetRepository<CartItemEntity, int>();
                //cartItemRepo.Update(existingCartItem);
            }
            else if (existingCart != null && existingCartItem == null)
            {
                if (dto.CustomizedPreviewUrl != null)
                    uploadedImageUrl = await attachmentService.UploadImageAsync(dto.CustomizedPreviewUrl, $"CartItems/{targetCartId}");

                await HandleExistingCartWithFoundObjectAsync(existingCart, dto, variant, userId, uploadedImageUrl);
            }
            else
            {
                if (dto.CustomizedPreviewUrl != null)
                    uploadedImageUrl = await attachmentService.UploadImageAsync(dto.CustomizedPreviewUrl, $"CartItems/{targetCartId}");

                await CreateNewCartWithFixedIdAsync(targetCartId, dto, userId, uploadedImageUrl);
            }

            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                if (!string.IsNullOrEmpty(uploadedImageUrl))
                    await attachmentService.DeleteImage(uploadedImageUrl);

                throw new BadRequestExceptionCustome("حدث خطأ أثناء حفظ البيانات في السلة.");
            }

            return await GetCartAsync(targetCartId);
        }
        public async Task<ShoppingCartDto> UpdateItemQuantityAsync(Guid cartId, int cartItemId, UpdateCartItemQuantityDto dto)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();

            // 1. we need to get the cart with its items and related product details to validate the stock
            // without making extra call to the database for fetching the product variant details because we already have them in the cart items
            var cartSpec = new GetShoppingCartSpec(cartId);
            var cart = await cartRepository.GetByIdWithSpecAsync(cartSpec);

            if (cart == null)
                throw new ShoppingCartNotFoundException("Cart Not Found");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem == null)
                throw new NotFoundExceptionCustome("Cart Item Not Found");

            if (dto.Quantity <= 0)
                throw new BadRequestExceptionCustome("Quantity must be greater than zero");

            // no call the database to validate the stock because we already have the product variant details in the cart item entity
            ValidateProductStockInMemory(cartItem, dto.Quantity);

            // 3. update the quantity and save changes
            cartItem.Quantity = dto.Quantity;
            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastModifiedBy = cart.UserId;

            //cartRepository.Update(cart); // ensure the cart is tracked for update

            await _unitOfWork.SaveChangesAsync();

            return await GetCartAsync(cartId);
        }

        public async Task<ShoppingCartDto> RemoveItemFromCartAsync(Guid shoppingCartId, int cartItemId)
        {
            // 1 - check existence 
            var cartRepo = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();

            // get the cart with its items to find the item to delete and also to update the cart after deleting the item
            var cartItemRepo = _unitOfWork.GetRepository<CartItemEntity, int>();

            var cartSpec = new GetShoppingCartSpec(shoppingCartId);
            var cart = await cartRepo.GetByIdWithSpecAsync(cartSpec);

            if (cart == null) throw new ShoppingCartNotFoundException("Shopping Cart Not Found");

            // 2 - find the item in the cart
            var item = cart.CartItems.FirstOrDefault(c => c.Id == cartItemId);
            if (item == null) throw new CartItemNotFound($"Cart Item Not Found");

            // 3 - delete image from server
            if (!string.IsNullOrEmpty(item.CustomizedPreviewUrl))
                await attachmentService.DeleteImage(item.CustomizedPreviewUrl);

            // 2. delete the item from the cart and database
            cart.CartItems.Remove(item); 
            cartItemRepo.Delete(item);  

            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastModifiedBy = cart.UserId;

            //cartRepo.Update(cart);

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
            var variantRepo = _unitOfWork.GetRepository<ProductVariantEntity, int>();

            var variant = await variantRepo.GetByIdAsync(variantId);

            if (variant == null)
                throw new NotFoundExceptionCustome("هذا المنتج غير متاح حالياً في النظام.");

            if (variant.StockQuantity < requestedQuantity)
                throw new BadRequestExceptionCustome(
                    $"الكمية المطلوبة غير متوفرة. المتاح في المخزن حالياً هو ({variant.StockQuantity}) قطعة فقط.");

            return variant;
        }
        private void ValidateProductStockInMemory(CartItemEntity cartItem, int requestedQuantity)
        {
            if (cartItem.ProductVariant == null)
                throw new NotFoundExceptionCustome("Product Details Not Loaded");

            if (cartItem.ProductVariant.StockQuantity < requestedQuantity)
                throw new BadRequestExceptionCustome($"Quantity : {cartItem.ProductVariant.StockQuantity} Not Available");
        }
        private async Task HandleExistingCartWithFoundObjectAsync(ShoppingCartEntity cart, AddToCartDto dto, ProductVariantEntity variant, string? userId, string? imageUrl)
        {
            if (cart == null)
                throw new ShoppingCartNotFoundException("Not Found Cart");

            var existingCartItem = cart.CartItems.FirstOrDefault(ci =>
                ci.ProductVariantId == dto.ProductVariantId &&
                ci.DesignId == dto.DesignId);

            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity + dto.Quantity > variant.StockQuantity)
                    throw new BadRequestExceptionCustome("Total Quantity Not Exist for this proudct");

                existingCartItem.Quantity += dto.Quantity;

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    if (!string.IsNullOrEmpty(existingCartItem.CustomizedPreviewUrl))
                    {
                        await attachmentService.DeleteImage(existingCartItem.CustomizedPreviewUrl);
                    }
                    existingCartItem.CustomizedPreviewUrl = imageUrl;
                }
            }
            else
            {
                var newCartItem = new CartItemEntity
                {
                    ShoppingCartId = cart.Id, 
                    ProductVariantId = dto.ProductVariantId,
                    Quantity = dto.Quantity,
                    DesignId = dto.DesignId,
                    CustomizedPreviewUrl = imageUrl,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                };

                cart.CartItems.Add(newCartItem);
                //var cartItemRepository = _unitOfWork.GetRepository<CartItemEntity, int>();
                //await cartItemRepository.AddAsync(newCartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastModifiedBy = userId;

        }
        private async Task CreateNewCartWithFixedIdAsync(Guid cartId, AddToCartDto dto, string? userId, string? imageUrl)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();

            var newCart = new ShoppingCartEntity
            {
                Id = cartId, 
                UserId = userId,
                CartItems = new List<CartItemEntity>
                {
                    new CartItemEntity
                    {
                        ProductVariantId = dto.ProductVariantId,
                        Quantity = dto.Quantity,
                        DesignId = dto.DesignId,
                        CustomizedPreviewUrl = imageUrl,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                    }
                }
            };

            await cartRepository.AddAsync(newCart);

        }
        #endregion

        #region Helper in Merging cart
        // merging the 2 carts
        private void MergeCartItemsWithStockValidation(ShoppingCartEntity currentGuestCart, ShoppingCartEntity oldUserCart, string userId)
        {
            foreach (var oldUserCartItem in oldUserCart.CartItems)
            {
                var existingUserItem = currentGuestCart.CartItems.FirstOrDefault(o =>
                    o.ProductVariantId == oldUserCartItem.ProductVariantId &&
                    o.DesignId == oldUserCartItem.DesignId); 

                if (existingUserItem != null)
                {
                    // total quantity
                    var requestedTotal = existingUserItem.Quantity + oldUserCartItem.Quantity;
                    // the maximum available quantity in DB
                    var availableStock = oldUserCartItem.ProductVariant.StockQuantity;

                    existingUserItem.Quantity = Math.Min(requestedTotal, availableStock);
                }
                else
                {
                    // if not exist item
                    currentGuestCart.CartItems.Add(new CartItemEntity
                    {
                        ProductVariantId = oldUserCartItem.ProductVariantId,
                        Quantity = oldUserCartItem.Quantity,
                        DesignId = oldUserCartItem.DesignId,
                        CustomizedPreviewUrl = oldUserCartItem.CustomizedPreviewUrl,
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

            return new ShoppingCartDto
            {
                Id = Guid.Empty, 
                CartItems = new List<CartItemDto>(),
            };
        }
        // assign the cart to the user and return it
        private async Task AssignCartToUserAsync(ShoppingCartEntity cart, string userId)
        {
            var cartRepository = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();

            cart.UserId = userId;
            cart.LastModifiedBy = userId;
            cart.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
