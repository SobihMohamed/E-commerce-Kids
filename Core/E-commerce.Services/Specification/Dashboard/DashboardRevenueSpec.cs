using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Order;
using E_commerce.Shared.Common.Params.Dashboard;
using E_commerce.Shared.EnumsHelper.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Dashboard
{
    public class DashboardRevenueSpec : BaseSpecifications<OrderEntity, Guid>
    {
            public DashboardRevenueSpec(DashboardParams specParams)
                : base(o =>
                    // لازم الطلب ميكونش ملغي أو فاشل (افترض عندك Enum لحالة الطلب)
                     o.OrderStatus == OrderStatus.Delivered &&
                    (!specParams.Days.HasValue || o.CreatedAt >= DateTime.UtcNow.AddDays(-specParams.Days.Value)) &&
                    (!specParams.StartDate.HasValue || o.CreatedAt >= specParams.StartDate.Value) &&
                    (!specParams.EndDate.HasValue || o.CreatedAt <= specParams.EndDate.Value)
                )
            {
            }
    }
}
