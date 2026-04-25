using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Common.Params.Notification
{
    public class NotificationQueryParams : BaseQueryParams
    {
            // null = هات الكل
            // true = هات المقروء بس
            // false = هات اللي لسه متقرأش بس
            public bool? IsRead { get; set; }
    }
}
