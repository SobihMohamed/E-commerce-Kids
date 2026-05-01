using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Order
{
    public class OrderItemDto
    {
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }

        // ==========================================
        //  NEW: Customization Details
        // ==========================================
        public int? DesignId { get; set; }
        public string? DesignName { get; set; }
        public decimal CustomizationPrice { get; set; }
        public string? CustomizedDesignUrl { get; set; }
        public decimal TotalItemPrice => (ProductPrice + CustomizationPrice) * Quantity;
    }
}
