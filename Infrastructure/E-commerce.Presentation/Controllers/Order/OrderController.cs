using E_commerce.Abstraction.IService.Order;
using E_commerce.Shared.Dto_s.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Presentation.Controllers.Order
{
    public class OrderController(IOrderService orderService) : AppBaseController
    {
        // 1.  create order
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateOrderAsync([FromBody] OrderToCreateDto orderDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdOrder = await orderService.CreateOrderAsync(userId!, orderDto);

            return Created(createdOrder, "Order created successfully.");
        }

        // 2. (Summary)
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetOrdersForUserAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await orderService.GetOrdersForUserAsync(userId!);

            return Success(orders);
        }

        // 3.(Details)
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderByIdForUserAsync(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await orderService.GetOrderByIdForUserAsync(id, userId!);

            return Success(order);
        }

        // 4. update order status (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateOrderStatusAsync(Guid id, [FromBody] UpdateOrderStatusDto statusDto)
        {
            var updatedOrder = await orderService.UpdateOrderStatusAsync(id, statusDto);
            return Success(updatedOrder, "Order status has been updated successfully.");
        }
    }
}