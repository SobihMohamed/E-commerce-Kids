using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Product
{
    public class ProductsByCategoryIdSpec : BaseSpecifications<ProductEntity,int>
    {
        public ProductsByCategoryIdSpec(int categoryId)
            : base(p => p.CategoryId == categoryId && p.IsBaseGarment == false)
        {
            
        }
    }
}
