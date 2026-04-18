using E_commerce.Domain.Models.Designs;
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
        public virtual ShoppingCartEntity ShoppingCart { get; set; } = null!;

        public int ProductVariantId { get; set; }
        public virtual ProductVariantEntity ProductVariant { get; set; } = null!;

        // ==========================================
        // 🌟 NEW: Customization Details
        // ==========================================
        public int? DesignId { get; set; } // Nullable عشان لو التيشرت سادة
        public virtual DesignsEntity? Design { get; set; }
    }
}
