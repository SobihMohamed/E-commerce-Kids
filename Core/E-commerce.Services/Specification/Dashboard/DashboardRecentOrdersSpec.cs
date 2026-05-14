using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Order;
using E_commerce.Shared.Common.Params.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Dashboard
{
    public class DashboardRecentOrdersSpec : BaseSpecifications<OrderEntity, Guid>
    {
        public DashboardRecentOrdersSpec(DashboardParams specParams)
            : base(o =>
                // فلترة بالأيام لو اتبعتت في الـ Params
                (!specParams.Days.HasValue || o.CreatedAt >= DateTime.UtcNow.AddDays(-specParams.Days.Value)) &&
                // فلترة بتواريخ مخصصة لو العميل اختارها
                (!specParams.StartDate.HasValue || o.CreatedAt >= specParams.StartDate.Value) &&
                (!specParams.EndDate.HasValue || o.CreatedAt <= specParams.EndDate.Value)
            )
        {
            // 1. Include User (عشان نجيب CustomerName) 
            // افترض إن العلاقة عندك اسمها User أو AppUser
            AddInclude(o => o.User);

            // 2. OrderBy (ترتيب تنازلي عشان نجيب أحدث حاجة)
            AddOrderBy(o => o.CreatedAt, isDescending: true);

            // 3. Pagination (نجيب العدد المطلوب بس، اللي هو الديفولت 5، والصفحة رقم 1)
            ApplyPagenation(specParams.RecentOrdersCount, 1);
        }
    }
}