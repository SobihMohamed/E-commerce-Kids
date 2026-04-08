using E_commerce.Shared.Dto_s.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Order
{
    public interface IOrderService
    {
        // the main operations related to orders that we need in our e-commerce application:

        // 1 - create order
        Task<OrderDto> CreateOrderAsync(string userId, OrderToCreateDto orderDto);
        
        // 2 - get all orders for a specific user 
        Task<IReadOnlyList<OrderDto>> GetOrdersForUserAsync(string userId);

        // 3 - get order details by order id for a specific user (to ensure users can only access their own orders) 
        Task<OrderDto> GetOrderByIdForUserAsync(Guid orderId, string userId);

        // 4 - update order status (for admin, e.g., from Pending to Shipped)
        Task<OrderDto> UpdateOrderStatusAsync(Guid orderId, string newStatus);
    }
}
