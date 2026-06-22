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
        public string ColorName { get; set; } = string.Empty;
        public string SizeName { get; set; } = string.Empty;
        public string? CustomizedPreviewUrl { get; set; }

        // ==========================================
        //  NEW: Customization Details
        // ==========================================
        public int? DesignId { get; set; }
        public string? DesignName { get; set; }
        public string? CustomizedDesignUrl { get; set; }

        // ❌ تم مسح الـ CustomizationPrice من هنا نهائياً
        // public decimal CustomizationPrice { get; set; }

        // 🌟 UPDATED: تم تعديل المعادلة عشان تحسب سعر المنتج فقط في الكمية
        public decimal TotalItemPrice => ProductPrice * Quantity;
    }
}