using E_commerce.Shared.Dto_s.Address;
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
        public DateTime CreatedAt { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public AddressDto ShippingAddress { get; set; } = null!;
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
