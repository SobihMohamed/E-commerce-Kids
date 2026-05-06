using E_commerce.Shared.EnumsHelper.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Common.Params.Order
{
    public class AdminOrderParams : BaseQueryParams
    {
        public OrderStatus? Status { get; set; }

        public DateTime? OrderDate { get; set; }
    }
}
