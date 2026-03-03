using E_commerce.Domain.Models.Product;


namespace E_commerce.Domain.Models.Order
{
    public class OrderItemEntity : BaseEntity<int>
    {
        public Guid OrderId { get; set; }
        public virtual OrderEntity Order { get; set; } = null!;

        public int ProductVariantId { get; set; }
        public virtual ProductVariantEntity ProductVariant { get; set; } = null!;

        // take snapshot of some product details at the time of order 
        // to ensure we have the correct info even if product details change later
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; } 
        public int Quantity { get; set; }
    }
}
