using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Product
{
    public class ProductByIdSpec : BaseSpecifications<ProductEntity, int>
    {
        public ProductByIdSpec(int id):
            base(p  => p.Id == id)
        {
            AddInclude(p => p.Category);
            AddInclude(p => p.Images);
            AddInclude(p => p.Variants);
            var SizeNavigation = $"{nameof(ProductEntity.Variants)}.{nameof(ProductVariantEntity.Size)}"; // ProductEntity.Variant.Size
            IncludeStrings.Add(SizeNavigation);

            var ColorNavigation = $"{nameof(ProductEntity.Variants)}.{nameof(ProductVariantEntity.Color)}"; // ProductEntity.Variant.Size
            IncludeStrings.Add(ColorNavigation);
        }
    }
}
