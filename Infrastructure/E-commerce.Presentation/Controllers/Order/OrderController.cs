using E_commerce.Abstraction.IService.Order;
using E_commerce.Shared.Common.Responses;
using E_commerce.Shared.Dto_s.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_commerce.Presentation.Controllers.Order
{
    public class OrderController(IOrderService orderService) : AppBaseController
    {
        // 1. Create Order
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OrderDto>), 201)] 
        public async Task<ActionResult> CreateOrderAsync([FromBody] OrderToCreateDto orderDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdOrder = await orderService.CreateOrderAsync(userId!, orderDto);

            return Created(createdOrder, "Order created successfully.");
        }

        // 2. Get User Orders (Summary)
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrderSummaryDto>>), 200)] 
        public async Task<ActionResult> GetOrdersForUserAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await orderService.GetOrdersForUserAsync(userId!);

            return Success(orders);
        }

        // 3. Get Order Details
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<OrderDto>), 200)] 
        public async Task<ActionResult> GetOrderByIdForUserAsync(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await orderService.GetOrderByIdForUserAsync(id, userId!);

            return Success(order);
        }

        // 4. Update Order Status (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(ApiResponse<OrderDto>), 200)] 
        public async Task<ActionResult> UpdateOrderStatusAsync(Guid id, [FromBody] UpdateOrderStatusDto statusDto)
        {
            var updatedOrder = await orderService.UpdateOrderStatusAsync(id, statusDto);
            return Success(updatedOrder, "Order status has been updated successfully.");
        }
    }
}