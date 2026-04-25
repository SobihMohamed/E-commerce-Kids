using E_commerce.Domain.Models.User;
using E_commerce.Shared.EnumsHelper.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Notification
{
    public class NotificationEntity : BaseEntity<Guid>
    {
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public string? SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }

        public NotificationType NotificationType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public string? ReferenceId { get; set; } // Link for the frontend to navigate to the relevant page (e.g., order details, chat message, etc.)
    }
}
