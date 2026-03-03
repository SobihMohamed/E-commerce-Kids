using E_commerce.Domain.Contracts;
using E_commerce.Domain.Models.Address;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Domain.Models.Notification;
using E_commerce.Domain.Models.Order;
using E_commerce.Shared.EnumsHelper.User;
using Microsoft.AspNetCore.Identity;

namespace E_commerce.Domain.Models.User
{
    public class ApplicationUser : IdentityUser , IEntity<string>
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserType UserType { get; set; } = UserType.Customer;
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<AddressEntity> Addresses { get; set; } = new HashSet<AddressEntity>();
        public virtual ICollection<OrderEntity> Orders { get; set; } = new HashSet<OrderEntity>();
        public virtual ICollection<NotificationEntity> Notifications { get; set; } = new HashSet<NotificationEntity>();
        public virtual ICollection<ProductReviewEntity> Reviews { get; set; } = new HashSet<ProductReviewEntity>();
        public virtual ShoppingCartEntity? ShoppingCart { get; set; }

    }
}
