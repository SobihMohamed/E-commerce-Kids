using E_commerce.Abstraction.IService.ShoppingCart;
using E_commerce.Shared.Dto_s.ShoppingCart.RequestDto;
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

        // 1. get cart
        [HttpGet("{cartId}")]
        public async Task<ActionResult> GetCart(Guid cartId)
        {
            var cart = await _shoppingCartService.GetCartAsync(cartId);
            return Success(cart);
        }

        // 2. add item to cart
        [HttpPost("items")]
        public async Task<ActionResult> AddItemToCart([FromBody] AddToCartDto dto)
        {
            // to associate the cart item with the user, we need to get the user ID from the token (if the user is logged in)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _shoppingCartService.AddItemToCartAsync(dto, userId);
            return Success(cart, "Item Added to cart Successfully");
        }

        // 3. update item quantity in cart
        [HttpPut("{cartId}/items/{cartItemId}")]
        public async Task<ActionResult> UpdateItemQuantity(Guid cartId, int cartItemId, [FromBody] UpdateCartItemQuantityDto dto)
        {
            var cart = await _shoppingCartService.UpdateItemQuantityAsync(cartId, cartItemId, dto);
            return Success(cart, "Item Updated Successfully");
        }

        // 4. delete item from cart
        [HttpDelete("{cartId}/items/{cartItemId}")]
        public async Task<ActionResult> RemoveItemFromCart(Guid cartId, int cartItemId)
        {
            var cart = await _shoppingCartService.RemoveItemFromCartAsync(cartId, cartItemId);
            return Success(cart, "Item Deleted From Cart Successfully");
        }

        // 5. Merge guest cart to user cart after login
        [Authorize]// This endpoint requires authentication to ensure that only logged-in users can merge their carts
        [HttpPost("merge")]
        public async Task<ActionResult> MergeCart([FromQuery] Guid guestCartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return UnauthorizedError("please login first");

            var cart = await _shoppingCartService.MergeGuestCartToUserCartAsync(guestCartId, userId);
            return Success(cart, "Merge Success");
        }

        // 6. Clear cart
        [HttpDelete("{cartId}")]
        public async Task<ActionResult> ClearCart(Guid cartId)
        {
            await _shoppingCartService.ClearCartAsync(cartId);
            return Success("Clear Cart Success");
        }
    }
}
