using E_commerce.Shared.EnumsHelper.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.User
{
    public class ApplicationUser : BaseEntity<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public UserType Role { get; set; } = UserType.Customer;
    }
}
