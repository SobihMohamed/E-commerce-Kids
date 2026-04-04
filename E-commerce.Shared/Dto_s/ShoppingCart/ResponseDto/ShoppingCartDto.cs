using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.ShoppingCart.ResponseDto
{
    public class ShoppingCartDto
    {
        // Why Nullable? Because first time user adds to cart,
        // the front store will create a new cart and associate it with the user (if logged in) or session.
        public Guid? Id { get; set; }
        public string? UserId { get; set; } // optional because user can add to cart without being logged in,

        public List<CartItemDto> CartItems { get; set; } = new();

        // Calculated properties for total items and total price in the cart
        public int totalItems => CartItems.Sum(i => i.Quantity);
        public decimal totalPrice => CartItems.Sum(i => i.SubTotal);
    }
}
