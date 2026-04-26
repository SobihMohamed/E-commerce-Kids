using E_commerce.Shared.Dto_s.Product.Variant;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Shared.Dto_s.Product
{
    public class ProductToCreateDto
    {
        [Required(ErrorMessage = "Product Name is Required")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Must greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Required main image")]
        public IFormFile MainImageUrl { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }
        public List<IFormFile>? ImageUrls { get; set; }

        [Required(ErrorMessage = "Must Add at least on Variant")]
        public List<VariantToCreateDto> Variants { get; set; } = new();
    }
}
