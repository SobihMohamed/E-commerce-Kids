using E_commerce.Domain.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Product
{
    public class ProductEntity : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MainImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // TargetAge
        public string TargetAge { get; set; } = string.Empty;

        // Foreign Key
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;

        // Navigation Properties
        public ICollection<ProductImageEntity> Images { get; set; } = new List<ProductImageEntity>();
        public ICollection<ProductVariantEntity> Variants { get; set; } = new List<ProductVariantEntity>();
    }
}
