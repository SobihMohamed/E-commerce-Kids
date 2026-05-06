using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Order
{
    public class AdminOrderByIdSpec : BaseSpecifications<OrderEntity, Guid>
    {
        public AdminOrderByIdSpec(Guid orderId) : base(o => o.Id == orderId)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.ShippingAddress);
            AddInclude(o => o.User);
        }
    }
}
