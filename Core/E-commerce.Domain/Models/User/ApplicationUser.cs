using E_commerce.Shared.EnumsHelper.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.User
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserType Role { get; set; } = UserType.Customer;
        public bool IsDeleted { get; set; } = false;
    }
}
