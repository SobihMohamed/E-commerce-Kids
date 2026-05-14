using E_commerce.Abstraction.IService.ShoppingCart;
using E_commerce.Shared.Common.Responses; 
using E_commerce.Shared.Dto_s.ShoppingCart.RequestDto;
using E_commerce.Shared.Dto_s.ShoppingCart.ResponseDto; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Presentation.Controllers.ShoppingCart
{
    public class ShoppingCartController : AppBaseController
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // 1. Get Cart
        [HttpGet("{cartId}")]
        [ProducesResponseType(typeof(ApiResponse<ShoppingCartDto>), 200)]
        public async Task<ActionResult> GetCart(Guid cartId)
        {
            var cart = await _shoppingCartService.GetCartAsync(cartId);
            return Success(cart);
        }

        // 2. Add Item to Cart
        [HttpPost("items")]
        [ProducesResponseType(typeof(ApiResponse<ShoppingCartDto>), 200)]
        public async Task<ActionResult> AddItemToCart([FromForm] AddToCartDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _shoppingCartService.AddItemToCartAsync(dto, userId);
            return Success(cart, "Item Added to cart Successfully");
        }

        // 3. Update Item Quantity
        [HttpPut("{cartId}/items/{cartItemId}")]
        [ProducesResponseType(typeof(ApiResponse<ShoppingCartDto>), 200)]
        public async Task<ActionResult> UpdateItemQuantity(Guid cartId, int cartItemId, [FromBody] UpdateCartItemQuantityDto dto)
        {
            var cart = await _shoppingCartService.UpdateItemQuantityAsync(cartId, cartItemId, dto);
            return Success(cart, "Item Updated Successfully");
        }

        // 4. Delete Item from Cart
        [HttpDelete("{cartId}/items/{cartItemId}")]
        [ProducesResponseType(typeof(ApiResponse<ShoppingCartDto>), 200)]
        public async Task<ActionResult> RemoveItemFromCart(Guid cartId, int cartItemId)
        {
            var cart = await _shoppingCartService.RemoveItemFromCartAsync(cartId, cartItemId);
            return Success(cart, "Item Deleted From Cart Successfully");
        }

        // 5. Merge Guest Cart to User Cart
        [Authorize]
        [HttpPost("merge")]
        [ProducesResponseType(typeof(ApiResponse<ShoppingCartDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        public async Task<ActionResult> MergeCart([FromQuery] Guid guestCartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return UnauthorizedError("please login first");

            var cart = await _shoppingCartService.MergeGuestCartToUserCartAsync(guestCartId, userId);
            return Success(cart, "Merge Success");
        }

        // 6. Clear Cart
        [HttpDelete("{cartId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<ActionResult> ClearCart(Guid cartId)
        {
            await _shoppingCartService.ClearCartAsync(cartId);
            return Success("Clear Cart Success");
        }
    }
}