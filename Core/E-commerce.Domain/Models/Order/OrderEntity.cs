using E_commerce.Domain.Models.Address;
using E_commerce.Domain.Models.User;
using E_commerce.Shared.EnumsHelper.Order;

namespace E_commerce.Domain.Models.Order
{
    public class OrderEntity : BaseEntity<Guid>
    {

        public string OrderNumber { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }    
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; } 

        public int ShippingAddressId { get; set; }
        public AddressEntity ShippingAddress { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public List<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
    }
}
