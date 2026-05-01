using E_commerce.Abstraction.IService.Dashboard;
using E_commerce.Shared.Common.Params.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Presentation.Controllers.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : AppBaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // 1. GET DASHBOARD OVERVIEW
        // URL: api/Dashboard/overview
        [HttpGet("overview")]
        public async Task<ActionResult> GetDashboardOverview([FromQuery] DashboardParams dashboardParams)
        {
            var overview = await _dashboardService.GetDashboardOverviewAsync(dashboardParams);

            return Success(overview, "Dashboard overview retrieved successfully");
        }

        // 2. GET SALES ANALYTICS (Custom Date Range)
        // URL: api/Dashboard/sales-analytics?startDate=2026-04-01&endDate=2026-05-01
        [HttpGet("sales-analytics")]
        public async Task<ActionResult> GetSalesAnalytics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequestError("Start date cannot be greater than end date.");

            var analytics = await _dashboardService.GetSalesAnalyticsAsync(startDate, endDate);

            return Success(analytics, "Sales analytics retrieved successfully");
        }
    }
}