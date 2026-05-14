using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Category
{
    public class RegularCategorySpec : BaseSpecifications<CategoryEntity, int>
    {
        public RegularCategorySpec() : base(c => c.IsBaseGarment == false)
        {
        }
        public RegularCategorySpec(int id) : base(c => c.Id == id && c.IsBaseGarment == false)
        {
        }
    }
}
