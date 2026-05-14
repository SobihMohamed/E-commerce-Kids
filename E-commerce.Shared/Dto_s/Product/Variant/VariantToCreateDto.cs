using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Product.Variant
{
    public class VariantToCreateDto
    {
        [Required]
        public int ColorId { get; set; }

        [Required]
        public int SizeId { get; set; }  

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Can not be lees than 0")]
        public int StockQuantity { get; set; } 
    }
}
