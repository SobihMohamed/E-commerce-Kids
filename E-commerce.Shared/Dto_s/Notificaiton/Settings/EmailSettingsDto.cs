using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Common.Dto.Notification.Settings
{
    public class EmailSettingsDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Port { get; set; }
    }
}
