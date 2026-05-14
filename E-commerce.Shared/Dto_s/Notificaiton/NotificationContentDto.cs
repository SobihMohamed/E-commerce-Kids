using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Notificaiton
{
    public class NotificationContentDto
    {
        public string UserId { get; set; } = string.Empty; // For DB and Push (SignalR)
        public string? Email { get; set; } // For Email Strategy (Nullable because not all notifications need emails)
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? ReferenceId { get; set; } // string here too
    }
}
