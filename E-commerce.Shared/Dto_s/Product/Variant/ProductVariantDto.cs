using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Product.Variant
{
    public class ProductVariantDto
    {
        public int VariantId { get; set; } 
        public string Size { get; set; } = string.Empty; 
        public string ColorName { get; set; } = string.Empty; 
        public string ColorHexCode { get; set; } = string.Empty; 
        public int StockQuantity { get; set; }
    }
}
