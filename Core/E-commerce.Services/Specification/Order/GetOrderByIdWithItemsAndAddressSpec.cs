using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Order
{
    public class GetOrderByIdWithItemsAndAddressSpec : BaseSpecifications<OrderEntity, Guid>
    {
        public GetOrderByIdWithItemsAndAddressSpec(Guid orderId , string userId)
            :base(o => o.Id == orderId && o.UserId == userId)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.ShippingAddress);
            AddInclude(o => o.User);
        }
    }
}
