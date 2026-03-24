using E_commerce.Shared.Dto_s.Product.Variant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Product
{
    public class ProductToUpdateDto
    {
        [MaxLength(200)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Must be greater than 0")]
        public decimal? Price { get; set; }

        public IFormFile? MainImageUrl { get; set; }

        public string? TargetAge { get; set; }

        public int? CategoryId { get; set; }

        public List<IFormFile>? ImageUrls { get; set; }

        public List<VariantToUpdateDto>? Variants { get; set; }
    }
}
