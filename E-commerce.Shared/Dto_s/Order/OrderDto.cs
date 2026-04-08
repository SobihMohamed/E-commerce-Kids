using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public int ShippingAddressId { get; set; }

        public DateTime CreatedAt { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
