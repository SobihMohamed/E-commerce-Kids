using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.ShoppingCart.RequestDto
{
    public class AddToCartDto
    {
        // Why Nullable? Because first time user adds to cart,
        // we create a new cart and associate it with the user (if logged in) or session.
        // So cartId will be null until the first item is added.
        public Guid? ShoppingCartId { get; set; }
        [Required]
        public int ProductVariantId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } = 1; // Default to 1 if not specified
    }
}
