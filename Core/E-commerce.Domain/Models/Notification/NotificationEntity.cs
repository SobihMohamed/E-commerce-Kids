using E_commerce.Domain.Models.User;
using E_commerce.Shared.EnumsHelper.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Notification
{
    public class NotificationEntity : BaseEntity<int>
    {
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public string? SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }

        public  NotificationType NotificationType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
    }
}
