using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Product.Variant
{
    public class VariantToUpdateDto
    {
        public int? Id { get; set; }

        public int? ColorId { get; set; }
        public int? SizeId { get; set; }

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }
    }
}
