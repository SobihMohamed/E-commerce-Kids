using E_commerce.Shared.EnumsHelper.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Common.Params.Product
{
    public class ProductSpecParams : BaseQueryParams
    {
        public int? CategoryId { get; set; }
        public int? ColorId { get; set; }
        public TargetGender? Gender { get; set; }
        public int? SizeId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public ProductArranges? Sort { get; set; }
        public bool? IsBaseGarment { get; set; }
    }
}
