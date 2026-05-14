using E_commerce.Shared.Common.Params.Dashboard;
using E_commerce.Shared.Dto_s.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Dashboard
{
    public interface IDashboardService
    {

        Task<DashboardOverviewDto> GetDashboardOverviewAsync(DashboardParams dashboardParams);

        Task<List<SalesChartDto>> GetSalesAnalyticsAsync(DateTime startDate, DateTime endDate);
    }
}
