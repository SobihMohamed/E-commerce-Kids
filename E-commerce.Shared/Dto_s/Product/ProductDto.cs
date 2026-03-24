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
        // font make it as circles 
        public List<string> AvailableColorHexCodes { get; set; } = new();
    }
}
