using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Order;
using E_commerce.Shared.Common.Params.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Dashboard
{
    public class DashboardChartsSpec : BaseSpecifications<OrderEntity, Guid>
    {
        public DashboardChartsSpec(DashboardParams specParams)
            : base(o =>
                (!specParams.Days.HasValue || o.CreatedAt >= DateTime.UtcNow.AddDays(-specParams.Days.Value))
            )
        {
        }
    }
}
