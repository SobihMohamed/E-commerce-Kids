using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Notificaiton
{
    public class MessageDto
    {
        public string To { get; set; } // email or phone
        public string Subject { get; set; } 
        public string Body { get; set; } 
    }
}
