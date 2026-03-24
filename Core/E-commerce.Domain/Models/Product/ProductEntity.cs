using E_commerce.Domain.Models.Category;
using E_commerce.Domain.Models.CustomerInteraction;

namespace E_commerce.Domain.Models.Product
{
    public class ProductEntity : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MainImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<ProductImageEntity> Images { get; set; } = new List<ProductImageEntity>();
        public virtual ICollection<ProductVariantEntity> Variants { get; set; } = new List<ProductVariantEntity>();
        public virtual ICollection<ProductReviewEntity> Reviews { get; set; } = new List<ProductReviewEntity>();
    }
}
