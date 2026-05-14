using AutoMapper;
using E_commerce.Abstraction.IService.Notification;
using E_commerce.Abstraction.IService.Order;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Domain.Models.Order;
using E_commerce.Domain.Models.Product;
using E_commerce.Domain.Models.User;
using E_commerce.Services.Specification.Order;
using E_commerce.Services.Specification.ShoppingCart;
using E_commerce.Shared.Common.Pagination;
using E_commerce.Shared.Common.Params.Order;
using E_commerce.Shared.Dto_s.Order;
using E_commerce.Shared.EnumsHelper.Order;
using Microsoft.AspNetCore.Identity;

namespace E_commerce.Services.Services.OrderImplementation
{
    public partial class OrderService(IUnitOfWork unitOfWork,
        IMapper mapper,
        INotificationService notificationService,
        UserManager<ApplicationUser> userManager) : IOrderService
    {
        public async Task<OrderDto> CreateOrderAsync(string userId, OrderToCreateDto orderDto)
        {
            // 1 - check if user has items in cart
            var shoppingCartRepo = unitOfWork.GetRepository<ShoppingCartEntity, Guid>();
            var shoppingSpec = new GetShoppingCartByUserIdSpec(userId);
            var shoppingCart = await shoppingCartRepo.GetByIdWithSpecAsync(shoppingSpec);

            if (shoppingCart == null || !shoppingCart.CartItems.Any())
                throw new ShoppingCartNotFoundException("Cart Not Found");

            CheckProductVariantAndUpdateStockInMemory(shoppingCart);

            // 3 - convert the cart items to order items
            var orderItems = ConvertCartItemToOrder(shoppingCart);

            // 4 - calculate the order subtotal 
            var subTotal = orderItems.Sum(x => (x.ProductPrice + x.CustomizationPrice) * x.Quantity);
            var shippingFee = 10m; // for example

            // 5 - create the order entity
            var orderEntity = new OrderEntity
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

            var savedOrderDto = await SaveOrderAndClearCartItemsInDb(orderEntity, shoppingCart);

            await NotifyOnOrderCreationAsync(orderEntity);

            return savedOrderDto;
        }

        #region Helper Methods in CreateOrderAsync

        private void CheckProductVariantAndUpdateStockInMemory(ShoppingCartEntity shoppingCart)
        {
            foreach (var item in shoppingCart.CartItems)
            {
                if (item.ProductVariant == null)
                    throw new NotFoundExceptionCustome("Product details not loaded.");

                if (item.ProductVariant.StockQuantity < item.Quantity)
                    throw new BadRequestExceptionCustome(
                        $"المنتج '{item.ProductVariant.Product.Name}' غير متوفر بالكمية المطلوبة. المتاح ({item.ProductVariant.StockQuantity}) فقط.");

                item.ProductVariant.StockQuantity -= item.Quantity;
            }
        }

        private List<OrderItemEntity> ConvertCartItemToOrder(ShoppingCartEntity shoppingCart)
        {
            var orderItems = new List<OrderItemEntity>();
            foreach (var item in shoppingCart.CartItems)
            {
                orderItems.Add(new OrderItemEntity
                {
                    ProductVariantId = item.ProductVariantId,
                    ProductName = item.ProductVariant.Product.Name,
                    ProductPrice = item.ProductVariant.Product.Price,
                    Quantity = item.Quantity,
                    ColorName = item.ProductVariant.Color?.Name ?? "N/A",
                    SizeName = item.ProductVariant.Size?.Name ?? "N/A",
                    // if no customized preview url, use the main image of the product as a fallback
                    CustomizedPreviewUrl = !string.IsNullOrEmpty(item.CustomizedPreviewUrl)
                        ? item.CustomizedPreviewUrl
                        : item.ProductVariant.Product.MainImageUrl,

                    DesignId = item.DesignId,
                    DesignName = item.Design?.Name,
                    CustomizedDesignUrl = item.Design?.ImageUrl,
                    CustomizationPrice = item.DesignId.HasValue ? 15.0m : 0m
                });
            }
            return orderItems;
        }
        // function to save the order and clear the cart items in the same transaction
        private async Task<OrderDto> SaveOrderAndClearCartItemsInDb(OrderEntity order, ShoppingCartEntity shoppingCart)
        {
            var orderRepo = unitOfWork.GetRepository<OrderEntity, Guid>();
            var cartItemRepo = unitOfWork.GetRepository<CartItemEntity, int>();

            // 👇 1. ضفنا الـ Repo الخاص بالمخزون
            var variantRepo = unitOfWork.GetRepository<ProductVariantEntity, int>();

            await orderRepo.AddAsync(order);

            var itemsToDelete = shoppingCart.CartItems.ToList();
            foreach (var item in itemsToDelete)
            {
                if (item.ProductVariant != null)
                {
                    item.ProductVariant.Product = null;
                    item.ProductVariant.Color = null;
                    item.ProductVariant.Size = null;

                    variantRepo.Update(item.ProductVariant);
                }

                item.ProductVariant = null;
                item.ShoppingCart = null;
                item.Design = null;

                cartItemRepo.Delete(item);
            }

            var res = await unitOfWork.SaveChangesAsync();

            if (res <= 0)
                throw new BadRequestExceptionCustome("Failed to create order");

            var spec = new OrderWithAllDetailsSpec(order.Id);
            var completeOrder = await orderRepo.GetByIdWithSpecAsync(spec);

            return mapper.Map<OrderDto>(completeOrder);
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
            var variantRepo = unitOfWork.GetRepository<ProductVariantEntity, int>();

            var spec = new AdminOrderByIdSpec(orderId);
            var order = await orderRepo.GetByIdWithSpecAsync(spec);

            if (order == null)
                throw new OrderNotFoundException($"Order not found.");

            // if new status is the same as the current status, we don't need to update
            if (order.OrderStatus == Dto.NewStatus)
                throw new BadRequestExceptionCustome("Cannot update status to the same value.");

            // if the new status is cancelled, we need to update the stock quantity of the product variants in the order items
            if (Dto.NewStatus == OrderStatus.Cancelled )
            {
                foreach (var item in order.OrderItems)
                {
                    var variant = await variantRepo.GetByIdAsync(item.ProductVariantId);
                    if (variant != null)
                    {
                        variant.StockQuantity += item.Quantity; 
                        variantRepo.Update(variant);
                    }
                }
            }

            order.OrderStatus = Dto.NewStatus;

            orderRepo.Update(order);
            var result = await unitOfWork.SaveChangesAsync();

            if (result <= 0)
                throw new BadRequestExceptionCustome("Failed to update order status.");

            await NotifyOnOrderStatusUpdateAsync(order);

            return mapper.Map<OrderDto>(order);
        }

        public async Task<PaginationResponse<OrderSummaryDto>> GetAllOrdersForAdminAsync(AdminOrderParams specParams)
        {
            var orderRepo = unitOfWork.GetRepository<OrderEntity, Guid>();

            // 1. Data Spec with filtering and pagination
            var spec = new AdminOrdersSpec(specParams);
            var orders = await orderRepo.GetAllWithSpecAsync(spec);

            // 2. Count Spec to get total items matching the filters
            var countSpec = new AdminOrderCountSpec(specParams);
            var totalItems = await orderRepo.GetCountAsync(countSpec);

            // 3. Mapping
            var mappedOrders = mapper.Map<IReadOnlyList<OrderSummaryDto>>(orders);

            // 4. Return Paginated Response
            return new PaginationResponse<OrderSummaryDto>(
                specParams.PageIndex,
                specParams.PageSize,
                totalItems,
                mappedOrders
            );
        }

        public async Task<OrderDto> GetOrderByIdForAdminAsync(Guid orderId)
        {
            // 1 - Get order repository
            var orderRepo = unitOfWork.GetRepository<OrderEntity, Guid>();

            // 2 - Create the specification
            var spec = new AdminOrderByIdSpec(orderId);

            // 3 - Get the order from the database
            var order = await orderRepo.GetByIdWithSpecAsync(spec);

            // 4 - Check if order exists
            if (order == null)
                throw new NotFoundExceptionCustome($"Order with ID {orderId} not found.");

            // 5 - Map and return
            return mapper.Map<OrderDto>(order);
        }
    }
}
