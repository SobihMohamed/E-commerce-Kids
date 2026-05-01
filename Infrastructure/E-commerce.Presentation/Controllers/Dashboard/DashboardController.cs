using E_commerce.Abstraction.IService.Dashboard;
using E_commerce.Shared.Common.Params.Dashboard;
using E_commerce.Shared.Common.Responses;
using E_commerce.Shared.Dto_s.Dashboard;
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
        [HttpGet("overview")]
        [ProducesResponseType(typeof(ApiResponse<DashboardOverviewDto>), 200)] 
        public async Task<ActionResult> GetDashboardOverview([FromQuery] DashboardParams dashboardParams)
        {
            var overview = await _dashboardService.GetDashboardOverviewAsync(dashboardParams);

            return Success(overview, "Dashboard overview retrieved successfully");
        }

        // 2. GET SALES ANALYTICS
        [HttpGet("sales-analytics")]
        [ProducesResponseType(typeof(ApiResponse<List<SalesChartDto>>), 200)]
        public async Task<ActionResult> GetSalesAnalytics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequestError("Start date cannot be greater than end date.");
            }

            var analytics = await _dashboardService.GetSalesAnalyticsAsync(startDate, endDate);

            return Success(analytics, "Sales analytics retrieved successfully");
        }
    }
}