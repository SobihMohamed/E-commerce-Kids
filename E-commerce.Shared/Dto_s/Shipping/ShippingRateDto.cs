using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Shipping
{
    public class ShippingRateDto
    {
        public int Id { get; set; }
        public string CityName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
