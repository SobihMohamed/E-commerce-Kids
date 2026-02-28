using E_commerce.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Address
{
    public class AddressEntity : BaseEntity<int>
    {
        public string City { get; set; } = string.Empty;
        public string StreetDetails { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsDefault { get; set; } 

        // Foreign Key
        public string UserId { get; set; } = string.Empty;

        // Navigation Property: Each address belongs to one user
        public ApplicationUser User { get; set; } = null!;
    }
}
