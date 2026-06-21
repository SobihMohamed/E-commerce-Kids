using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Shipping;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Shipping
{
    public class GetShippingRateByCityNameSpec : BaseSpecifications<ShippingRates, int>
    {
        public GetShippingRateByCityNameSpec(string cityName)
            : base(s => s.CityName == cityName)
        {
        }
    }
}
