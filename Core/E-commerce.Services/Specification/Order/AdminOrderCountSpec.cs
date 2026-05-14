using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Order;
using E_commerce.Shared.Common.Params.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Order
{
    public class AdminOrderCountSpec : BaseSpecifications<OrderEntity, Guid>
    {
        public AdminOrderCountSpec(AdminOrderParams specParams) : base(o =>
            // 1. Search in OrderNumber, Phone, or FullName
            (string.IsNullOrEmpty(specParams.Search) ||
             o.OrderNumber.ToLower().Contains(specParams.Search) ||
             o.ShippingAddress.PhoneNumber.Contains(specParams.Search) ||
             o.User.FullName.ToLower().Contains(specParams.Search)) &&

            // 2. Filter by Date 
            (!specParams.OrderDate.HasValue || o.CreatedAt.Date == specParams.OrderDate.Value.Date) &&

            // 3. Filter by Status
            (!specParams.Status.HasValue || o.OrderStatus == specParams.Status)
        )
        {

        }
    }
}
