using AutoMapper;
using E_commerce.Abstraction.IService.Order;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Domain.Models.Order;
using E_commerce.Domain.Models.Product;
using E_commerce.Services.Specification.Order;
using E_commerce.Services.Specification.ShoppingCart;
using E_commerce.Shared.Dto_s.Order;
using E_commerce.Shared.EnumsHelper.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.OrderImplementation
{
    public class OrderService(IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
    {
        public async Task<OrderDto> CreateOrderAsync(string userId, OrderToCreateDto orderDto) // orderDto contains the shipping address id
        {
            // 1 - check if user has items in cart
            var shoppingCartRepo = unitOfWork.GetRepository<ShoppingCartEntity, Guid>();
            var shoppingSpec = new GetShoppingCartByUserIdSpec(userId);
            var shoppingCart = await shoppingCartRepo.GetByIdWithSpecAsync(shoppingSpec);
            if (shoppingCart == null || !shoppingCart.CartItems.Any()) throw new ShoppingCartNotFoundException("Cart Not Found");

            // 2 - chcek if the product variant exist in the database and update the stock quantity
            await checkProductVariantandUpdateDb(shoppingCart);
            // 3 - convert the cart items to order items
            var orderItems = ConvertCartItemToOrder(shoppingCart);
            // 4 - calculate the order subtotal
            var subTotal = orderItems.Sum(x => x.ProductPrice * x.Quantity);
            var shippingFee = 10; // for example

            // 5 - create the order entity
            var OrderEntity = new OrderEntity
            {
                OrderNumber = $"ORD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                UserId = userId,
                ShippingAddressId = orderDto.ShippingAddressId,
                SubTotal = subTotal,
                ShippingFee = shippingFee,
                TotalAmount = subTotal + shippingFee,
                OrderStatus = OrderStatus.Pending,
                OrderItems = orderItems
            };
            // 6 - save the order to the database
            return await SaveOrderInDb(OrderEntity);
        }
        #region Helper Methods in CreateOrderAsync
        private async Task checkProductVariantandUpdateDb(ShoppingCartEntity shoppingCart)
        {
            var productVariantRepo = unitOfWork.GetRepository<ProductVariantEntity, int>();
            // loop through the cart items and check if the product variant exist in the database
            foreach (var item in shoppingCart.CartItems)
            {
                var dbProductVariant = await productVariantRepo.GetByIdAsync(item.ProductVariantId);
                if(dbProductVariant == null) 
                    throw new ProductVariantNotFound($"Product not found");
                if(dbProductVariant.StockQuantity < item.Quantity)
                    throw new ProductVariantNotFound($"Product {item.ProductVariant.Product.Name} with color {item.ProductVariant.Color} and size {item.ProductVariant.Size} has only {dbProductVariant.StockQuantity} items in stock");
                dbProductVariant.StockQuantity -= item.Quantity; // reduce the stock quantity by the ordered quantity
                
                productVariantRepo.Update(dbProductVariant); // update the product variant in the database
            }
        }
        private List<OrderItemEntity> ConvertCartItemToOrder(ShoppingCartEntity shoppingCart)
        {
            // 1- create the order items list
            var OrderItems = new List<OrderItemEntity>();
            // 2 - loop through the cart items and create the order items
            foreach (var item in shoppingCart.CartItems)
            {
                OrderItems.Add(new OrderItemEntity
                {
                    ProductVariantId = item.ProductVariantId,
                    ProductName = item.ProductVariant.Product.Name,
                    ProductPrice = item.ProductVariant.Product.Price,
                    Quantity = item.Quantity
                });
            }
            return OrderItems;
        }
        private async Task<OrderDto> SaveOrderInDb(OrderEntity order)
        {
            var orderRepo = unitOfWork.GetRepository<OrderEntity, Guid>();
            await orderRepo.AddAsync(order);
            var res = await unitOfWork.SaveChangesAsync();
            if(res <= 0) throw new BadRequestExceptionCustome("Failed to create order");
            return mapper.Map<OrderDto>(order);
        }
        #endregion
        
        public async Task<IReadOnlyList<OrderSummaryDto>> GetOrdersForUserAsync(string userId)
        {
            // 1- get order repository
            var orderRepo = unitOfWork.GetRepository<OrderEntity, Guid>();

            // 2 - get the orders for the user from the database
            var orderSpec = new GetOrdersForUserSpec(userId);
            var orders = await orderRepo.GetAllWithSpecAsync(orderSpec);
            
            // 3 - map the orders to order dto
            return mapper.Map<IReadOnlyList<OrderSummaryDto>>(orders);
        }

        

        public async Task<OrderDto> GetOrderByIdForUserAsync(Guid orderId, string userId)
        {
            // 1- get order repository
            var orderRepo = unitOfWork.GetRepository<OrderEntity, Guid>();

            // 2 - get the orders for the user from the database
            var orderSpec = new GetOrderByIdWithItemsAndAddressSpec(orderId , userId);
            var order = await orderRepo.GetByIdWithSpecAsync(orderSpec);

            if(order == null)
                throw new OrderNotFoundException($"Order not found.");

            // 3 - map the orders to order dto
            return mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusDto Dto)
        {
            var orderRepo = unitOfWork.GetRepository<OrderEntity, Guid>();

            var spec = new GetOrderByIdForAdminSpec(orderId);
            var order = await orderRepo.GetByIdWithSpecAsync(spec);

            if (order == null)
                throw new OrderNotFoundException($"Order not found.");

            order.OrderStatus = Dto.NewStatus;

            orderRepo.Update(order);
            var result = await unitOfWork.SaveChangesAsync();

            if (result <= 0)
                throw new BadRequestExceptionCustome("Failed to update order status.");

            return mapper.Map<OrderDto>(order);
        }
    }
}
