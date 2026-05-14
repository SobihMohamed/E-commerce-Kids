using E_commerce.Shared.EnumsHelper.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Order
{
    public class UpdateOrderStatusDto
    {
        public OrderStatus NewStatus { get; set; }
    }
}
