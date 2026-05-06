using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Product;
using E_commerce.Shared.EnumsHelper.Design;
using E_commerce.Shared.EnumsHelper.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Product
{
    public class BaseGarmentByGenderSpec : BaseSpecifications<ProductEntity, int>
    {
        public BaseGarmentByGenderSpec(TargetGender gender)
            : base(p => p.IsBaseGarment == true && p.TargetGender == gender)
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