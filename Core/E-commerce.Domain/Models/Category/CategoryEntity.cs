using E_commerce.Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Category
{
    public class CategoryEntity : BaseEntity<int>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string PictureUrl { get; set; } = string.Empty;

        // if true then this product is a base garment, and it will be used to create variants (like size, color, etc.)
        public bool IsBaseGarment { get; set; } = false;

        // Navigation Property: One Category has many Products
        public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
