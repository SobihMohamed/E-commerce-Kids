using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.NotificationSpecification
{
    public class UnreadNotificationsSpec : BaseSpecifications<NotificationEntity, Guid>
    {
        public UnreadNotificationsSpec(string userId)
            : base(n => n.UserId == userId && !n.IsRead)
        {
        }
    }
}
