using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.ShoppingCart.ResponseDto
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }

        // Product details for display purposes
        public string ProductName { get; set; } = string.Empty;
        public string MainImageUrl { get; set; } = string.Empty;
        public string ColorName { get; set; } = string.Empty;
        public string SizeName { get; set; } = string.Empty;

        // The price of the product variant at the time it was added to the cart
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int AvailableStock { get; set; } // Count of items available in stock for this product variant to prevent overselling

        // ==========================================
        // 🌟 NEW: Design display info & cost
        // ==========================================
        public int? DesignId { get; set; }
        public string? DesignName { get; set; }
        public string? DesignImageUrl { get; set; }

        public decimal SubTotal => UnitPrice * Quantity;
    }
}