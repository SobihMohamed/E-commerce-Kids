using E_commerce.Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Category
{
    public class CategoryEntity : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation Property: One Category has many Products
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
