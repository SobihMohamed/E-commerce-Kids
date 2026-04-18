using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Designs;
using E_commerce.Shared.EnumsHelper.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Design
{
    public class DesignByGenderFiltrationSpec : BaseSpecifications<DesignsEntity, int>
    {
        public DesignByGenderFiltrationSpec(DesignGender? designGender)
        : base(d => !designGender.HasValue || d.DesignGender == designGender)
        {
        }
    }
}
