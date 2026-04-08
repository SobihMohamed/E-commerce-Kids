using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Order
{
    public class OrderToCreateDto
    {
        public int ShippingAddressId { get; set; }
        // we get the cart id from the client because we need to know which cart we want to create the order from it
    }
}
