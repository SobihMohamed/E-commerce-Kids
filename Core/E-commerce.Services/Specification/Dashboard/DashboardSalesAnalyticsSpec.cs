using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Order;
using E_commerce.Shared.EnumsHelper.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Dashboard
{
    public class DashboardSalesAnalyticsSpec : BaseSpecifications<OrderEntity, Guid>
    {
        public DashboardSalesAnalyticsSpec(DateTime startDate, DateTime endDate)
            : base(o =>
                o.CreatedAt >= startDate &&
                o.CreatedAt <= endDate &&
                o.OrderStatus == OrderStatus.Delivered
            )
        {
        }
    }
}
