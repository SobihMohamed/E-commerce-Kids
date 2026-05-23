using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Auth.Sign_In_Up
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]{2,}@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{3,}$", ErrorMessage = "Invalid email format. Please use a valid domain (e.g., user@example.com)")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters long")]
        public string Password { get; set; } = null!;

        [Required, Phone]
        public string PhoneNumber { get; set; } = null!;
    }
}
