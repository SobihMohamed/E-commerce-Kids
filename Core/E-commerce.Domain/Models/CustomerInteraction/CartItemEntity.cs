using E_commerce.Domain.Models.Product;
using E_commerce.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.CustomerInteraction
{
    public class CartItemEntity : BaseEntity<int>
    {
        public int Quantity { get; set; }

        public Guid ShoppingCartId { get; set; }
        public ShoppingCartEntity ShoppingCartEntity { get; set; } = null!;

        public int ProductVariantId { get; set; }
        public ProductVariantEntity ProductVariant { get; set; } = null!;
    }
}
