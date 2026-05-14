using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Order;
using E_commerce.Domain.Models.Product;
using E_commerce.Shared.Common.Params.Dashboard;
using E_commerce.Shared.EnumsHelper.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Dashboard
{
    public class TopSellingProductsSpec : BaseSpecifications<OrderItemEntity, int>
    {
        public TopSellingProductsSpec(DashboardParams specParams)
            : base(item =>
                (!specParams.Days.HasValue || item.Order.CreatedAt >= DateTime.UtcNow.AddDays(-specParams.Days.Value)) &&
                 item.Order.OrderStatus == OrderStatus.Delivered
            )
        {
            var productInclude = $"{nameof(OrderItemEntity.ProductVariant)}.{nameof(ProductVariantEntity.Product)}";
            IncludeStrings.Add(productInclude);
        }
    }
}
