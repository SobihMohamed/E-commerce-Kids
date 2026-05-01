using AutoMapper;
using E_commerce.Abstraction.IService.Dashboard;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Models.Category;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Domain.Models.Designs;
using E_commerce.Domain.Models.Order;
using E_commerce.Domain.Models.Product;
using E_commerce.Services.Specification.Dashboard;
using E_commerce.Shared.Common.Params.Dashboard;
using E_commerce.Shared.Dto_s.Dashboard;
using E_commerce.Shared.EnumsHelper.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.DashboardImplementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DashboardOverviewDto> GetDashboardOverviewAsync(DashboardParams dashboardParams)
        {
            var overviewDto = new DashboardOverviewDto();

            // ==========================================================
            // 1. Counters (الأرقام والإحصائيات السريعة)
            // ==========================================================
            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var cartRepo = _unitOfWork.GetRepository<ShoppingCartEntity, Guid>();
            var orderRepo = _unitOfWork.GetRepository<OrderEntity, Guid>();
            var designRepo = _unitOfWork.GetRepository<DesignsEntity, int>();

            // استخدام الدالة الجديدة اللي عملناها
            overviewDto.Counters.TotalProducts = await productRepo.CountAsync();
            overviewDto.Counters.TotalCategories = await categoryRepo.CountAsync();
            overviewDto.Counters.TotalActiveCarts = await cartRepo.CountAsync();
            overviewDto.Counters.TotalDesigns = await designRepo.CountAsync();
            overviewDto.Counters.TotalOrders = await orderRepo.CountAsync();

            // ==========================================================
            // 2. Recent Orders & Total Revenue
            // ==========================================================

            // أ. حساب إجمالي المبيعات (Total Revenue)
            var revenueSpec = new DashboardRevenueSpec(dashboardParams);
            var revenueOrders = await orderRepo.GetAllWithSpecAsync(revenueSpec);
            // افترض إن الخاصية اللي شايلة الإجمالي في الـ OrderEntity اسمها TotalAmount
            overviewDto.Counters.TotalRevenue = revenueOrders.Sum(o => o.TotalAmount);

            // ب. جلب آخر الطلبات (Recent Orders)
            var recentOrdersSpec = new DashboardRecentOrdersSpec(dashboardParams);
            var recentOrders = await orderRepo.GetAllWithSpecAsync(recentOrdersSpec);

            // هنعمل Map للـ Entities دي عشان تتحول لـ RecentOrderDto
            overviewDto.RecentOrders = _mapper.Map<List<RecentOrderDto>>(recentOrders);

            // ==========================================================
            // 3. Top Products 
            // ==========================================================
            // هنجيب الـ Items باستخدام الـ Specification الجديدة
            var orderItemRepo = _unitOfWork.GetRepository<OrderItemEntity, int>(); // اتأكد من الـ PK type
            var topProductsSpec = new TopSellingProductsSpec(dashboardParams);
            var soldItems = await orderItemRepo.GetAllWithSpecAsync(topProductsSpec);

            // تجميع الداتا (لأن الـ Specification Pattern في الـ Generic Repo بيرجع TEntity دايماً)
            overviewDto.TopProducts = soldItems
                .GroupBy(i => i.ProductName) // أو i.ProductVariant.Product.Name حسب الـ Entity
                .Select(g => new TopSellingProductDto
                {
                    ProductName = g.Key,
                    TotalQuantitySold = g.Sum(i => i.Quantity),
                    TotalRevenueGenerated = g.Sum(i => (i.ProductPrice + i.CustomizationPrice) * i.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(dashboardParams.TopProductsCount)
                .ToList();

            // ==========================================================
            // 4. Charts Data 
            // ==========================================================
            var chartsSpec = new DashboardChartsSpec(dashboardParams);
            var chartsOrders = await orderRepo.GetAllWithSpecAsync(chartsSpec);

            overviewDto.OrderStatuses = chartsOrders
                .GroupBy(o => o.OrderStatus.ToString())
                .Select(g => new OrderStatusChartDto
                {
                    StatusName = g.Key,
                    Count = g.Count()
                })
                .ToList();

            // المبيعات اليومية للطلبات الناجحة بس
            overviewDto.SalesAnalytics = chartsOrders
                 .Where(o => o.OrderStatus == OrderStatus.Delivered) 
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new SalesChartDto
                {
                    DateLabel = g.Key.ToString("dd-MMM"),
                    OrdersCount = g.Count(),
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(c => c.DateLabel)
                .ToList();

            return overviewDto;
        }

        public async Task<List<SalesChartDto>> GetSalesAnalyticsAsync(DateTime startDate, DateTime endDate)
        {
            var orderRepo = _unitOfWork.GetRepository<OrderEntity, Guid>();

            var spec = new DashboardSalesAnalyticsSpec(startDate, endDate);
            var ordersInRange = await orderRepo.GetAllWithSpecAsync(spec);

            var salesAnalytics = ordersInRange
                .GroupBy(o => o.CreatedAt.Date) 
                .OrderBy(g => g.Key)           
                .Select(g => new SalesChartDto
                {
                    DateLabel = g.Key.ToString("dd-MMM"), 
                    OrdersCount = g.Count(),
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .ToList();

            return salesAnalytics;
        }
    }
}
