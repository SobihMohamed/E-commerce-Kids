using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Notificaiton
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ReferenceId { get; set; } // string here too
    }
}
