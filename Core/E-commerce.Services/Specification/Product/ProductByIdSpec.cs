using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Product
{
    public class ProductByIdSpec : BaseSpecifications<ProductEntity, int>
    {
        public ProductByIdSpec(int id, bool? isBaseGarment = null) :
            base(p => p.Id == id && (!isBaseGarment.HasValue || p.IsBaseGarment == isBaseGarment.Value))
        {
            AddInclude(p => p.Category);
            AddInclude(p => p.Images);
            AddInclude(p => p.Variants);

            var SizeNavigation = $"{nameof(ProductEntity.Variants)}.{nameof(ProductVariantEntity.Size)}";
            IncludeStrings.Add(SizeNavigation);

            var ColorNavigation = $"{nameof(ProductEntity.Variants)}.{nameof(ProductVariantEntity.Color)}";
            IncludeStrings.Add(ColorNavigation);
        }
    }
}
