using E_commerce.Domain.Models.Designs;
using E_commerce.Domain.Models.Product;


namespace E_commerce.Domain.Models.Order
{
    public class OrderItemEntity : BaseEntity<int>
    {
        public Guid OrderId { get; set; }
        public virtual OrderEntity Order { get; set; } = null!;

        public int ProductVariantId { get; set; }
        public virtual ProductVariantEntity ProductVariant { get; set; } = null!;
        public int? DesignId { get; set; } // Nullable: عشان لو اليوزر اشترى التيشرت سادة
        public virtual DesignsEntity? Design { get; set; }

        // Snapshots for Customization
        public string? DesignName { get; set; } 
        public decimal CustomizationPrice { get; set; } 
        public string? CustomizedDesignUrl { get; set; }

        // take snapshot of some product details at the time of order 
        // to ensure we have the correct info even if product details change later
        public string? CustomizedPreviewUrl { get; set; }
        public string ColorName { get; set; } = string.Empty; 
        public string SizeName { get; set; } = string.Empty;  
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; } 
        public int Quantity { get; set; }
    }
}
