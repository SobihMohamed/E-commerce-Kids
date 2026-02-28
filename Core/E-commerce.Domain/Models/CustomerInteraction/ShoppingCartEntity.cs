using E_commerce.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.CustomerInteraction
{
    public class ShoppingCartEntity : BaseEntity<Guid>
    {
        public Guid? UserId { get; set; } // Foreign Key to ApplicationUser
        // optional because user can add to cart without being logged in,
        // but if they are logged in , we want to associate the cart with their account
        public ApplicationUser? User { get; set; } = null!; // Navigation property
        public List<CartItemEntity> Items { get; set; } = new List<CartItemEntity>();
    }
}
