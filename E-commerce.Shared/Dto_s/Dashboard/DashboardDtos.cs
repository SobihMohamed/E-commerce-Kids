using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Dashboard
{
    public class DashboardOverviewDto
    {
        public DashboardCountersDto Counters { get; set; } = new();
        public List<SalesChartDto> SalesAnalytics { get; set; } = new();
        public List<OrderStatusChartDto> OrderStatuses { get; set; } = new();
        public List<TopSellingProductDto> TopProducts { get; set; } = new();
        public List<RecentOrderDto> RecentOrders { get; set; } = new();
    }
    public class DashboardCountersDto
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalDesigns { get; set; }
        public int TotalActiveCarts { get; set; } // سلال لم تتحول لطلبات
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; } // إجمالي المبيعات
    }
    public class SalesChartDto
    {
        public string DateLabel { get; set; } = string.Empty; // مثلا: "15-Oct" أو "October"
        public decimal Revenue { get; set; }
        public int OrdersCount { get; set; }
    }
    public class OrderStatusChartDto
    {
        public string StatusName { get; set; } = string.Empty; // Pending, Shipped, etc.
        public int Count { get; set; }
    }
    public class TopSellingProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenueGenerated { get; set; }
    }
    public class RecentOrderDto
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }
}
