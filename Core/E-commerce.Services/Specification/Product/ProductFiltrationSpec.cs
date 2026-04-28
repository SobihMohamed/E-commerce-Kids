using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Product;
using E_commerce.Shared.Common.Params.Product;
using E_commerce.Shared.EnumsHelper.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Product
{
    public class ProductFiltrationSpec : BaseSpecifications<ProductEntity, int>
    {
        public ProductFiltrationSpec(ProductSpecParams specParams) // search ? true || false
    : base(p =>
        p.IsBaseGarment == false &&
        // 1. Search where = product.Name Contain(search) AND Where product.categoryid = categoryid
        (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&

        // 2. Category T & F 
        (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId) &&

        // 3. Price t & f
        (!specParams.MinPrice.HasValue || p.Price >= specParams.MinPrice) && // price > 500
        (!specParams.MaxPrice.HasValue || p.Price <= specParams.MaxPrice) && // price  < 1000

        // 4. Variants
        (!specParams.ColorId.HasValue || p.Variants.Any(v => v.ColorId == specParams.ColorId)) &&
        (!specParams.SizeId.HasValue || p.Variants.Any(v => v.SizeId == specParams.SizeId))

    )
        {
            AddInclude(p => p.Category);
            AddInclude(p => p.Variants); 
            
            var SizeNavigation = $"{nameof(ProductEntity.Variants)}.{nameof(ProductVariantEntity.Size)}"; // ProductEntity.Variant.Size
            IncludeStrings.Add(SizeNavigation);
       
            var ColorNavigation = $"{nameof(ProductEntity.Variants)}.{nameof(ProductVariantEntity.Color)}"; // ProductEntity.Variant.Size
            IncludeStrings.Add(ColorNavigation);

            AddInclude(p => p.Images);

            if (specParams.Sort.HasValue) 
            {
                switch (specParams.Sort.Value) // priceAsc || priceDesc
                {
                    case ProductArranges.PriceAsc:
                        AddOrderBy(p => p.Price, isDescending: false);
                        break;
                    case ProductArranges.PriceDesc:
                        AddOrderBy(p => p.Price, isDescending: true);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name, isDescending: false); 
            }

            ApplyPagenation(specParams.PageSize, specParams.PageIndex);
        }
    }
}
