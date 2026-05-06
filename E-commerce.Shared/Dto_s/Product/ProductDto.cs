using E_commerce.Shared.Dto_s.Lookups.Color;
using E_commerce.Shared.Dto_s.Lookups.Size;
using E_commerce.Shared.Dto_s.Product.Variant;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string MainImageUrl { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        public string TargetGender { get; set; } = string.Empty;
        public bool IsBaseGarment { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new();
    }
}
