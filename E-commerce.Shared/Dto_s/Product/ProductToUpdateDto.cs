using E_commerce.Shared.Dto_s.Product.Variant;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Shared.Dto_s.Product
{
    public class ProductToUpdateDto
    {
        // 1. البيانات الأساسية الإجبارية (بنشيل منها الـ ? ونحط Required)
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public IFormFile? MainImageUrl { get; set; }

        public List<int>? DeletedImageIds { get; set; }

        public List<IFormFile>? ImageUrls { get; set; }

        public List<VariantToUpdateDto>? Variants { get; set; }
    }
}