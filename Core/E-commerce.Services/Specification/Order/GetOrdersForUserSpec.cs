using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Order
{
    public class GetOrdersForUserSpec : BaseSpecifications<OrderEntity,Guid>
    {
        public GetOrdersForUserSpec(string userId)
            :base(o => o.UserId == userId)
        {
            AddInclude(o => o.User); 
            AddInclude(o => o.ShippingAddress); 
            AddOrderBy(o => o.CreatedAt ,isDescending:true);
        }
    }
}
