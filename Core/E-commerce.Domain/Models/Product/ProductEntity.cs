using E_commerce.Domain.Models.Category;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Shared.EnumsHelper.Design;
using E_commerce.Shared.EnumsHelper.Product;

namespace E_commerce.Domain.Models.Product
{
    public class ProductEntity : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MainImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; } // +200 - 200
        public TargetGender TargetGender { get; set;  }
        public bool IsActive { get; set; } = true;

        // if true then this product is a base garment, and it will be used to create variants (like size, color, etc.)
        public bool IsBaseGarment { get; set; } = false;

        // Foreign Key 
        public int CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<ProductImageEntity> Images { get; set; } = new List<ProductImageEntity>();
        public virtual ICollection<ProductVariantEntity> Variants { get; set; } = new List<ProductVariantEntity>();
        public virtual ICollection<ProductReviewEntity> Reviews { get; set; } = new List<ProductReviewEntity>();
    }
}
