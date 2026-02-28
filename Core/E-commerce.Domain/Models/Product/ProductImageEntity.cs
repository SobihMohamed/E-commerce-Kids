using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Product
{
    public class ProductImageEntity : BaseEntity<int>
    {
        public string ImageUrl { get; set; } = string.Empty;

        // Foreign Key
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; } = null!;
    }
}
