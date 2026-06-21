using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Shipping
{
    public class ShippingRates : BaseEntity<int>
    {
        public string CityName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
