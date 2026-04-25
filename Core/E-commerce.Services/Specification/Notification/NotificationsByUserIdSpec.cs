using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Notification;
using E_commerce.Shared.Common.Params;
using E_commerce.Shared.Common.Params.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.NotificationSpecification
{
    public class NotificationsByUserIdSpec : BaseSpecifications<NotificationEntity,Guid>
    {
        public NotificationsByUserIdSpec(string userId , NotificationQueryParams queryParams)
            : base(n =>
                (n.UserId == userId) &&
                (!queryParams.IsRead.HasValue || n.IsRead == queryParams.IsRead.Value)
            )
        {
            ApplyPagenation(queryParams.PageSize ,queryParams.PageIndex);
            AddOrderBy(n => n.CreatedAt, isDescending: true);
        }
    }
}
