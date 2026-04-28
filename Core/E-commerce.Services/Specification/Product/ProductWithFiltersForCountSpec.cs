using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Product;
using E_commerce.Shared.Common.Params.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Product
{
    public class ProductWithFiltersForCountSpec : BaseSpecifications<ProductEntity, int>
    {
        public ProductWithFiltersForCountSpec(ProductSpecParams specParams)
            : base(p =>
                p.IsBaseGarment == false &&
                (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search.ToLower())) &&
                (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId) &&
                (!specParams.MinPrice.HasValue || p.Price >= specParams.MinPrice) &&
                (!specParams.MaxPrice.HasValue || p.Price <= specParams.MaxPrice) &&
                (!specParams.ColorId.HasValue || p.Variants.Any(v => v.ColorId == specParams.ColorId)) &&
                (!specParams.SizeId.HasValue || p.Variants.Any(v => v.SizeId == specParams.SizeId))
            )
        {
        }
    }
}
