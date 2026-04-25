using AutoMapper;
using E_commerce.Abstraction.IService.Notification;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.Notification;
using E_commerce.Services.Specification.NotificationSpecification;
using E_commerce.Shared.Common.Pagination;
using E_commerce.Shared.Common.Params.Notification;
using E_commerce.Shared.Dto_s.Notificaiton;
using E_commerce.Shared.EnumsHelper.Notification;


namespace E_commerce.Services.Services.NotificationImplementaion
{
    public class NotificationService(IEnumerable<INotificationStrategy> _notificationStrategies, IUnitOfWork unitOfWork, IMapper mapper) : INotificationService
    {
        public async Task SendNotificationAsync(NotificationContentDto message, params NotificationType[] types)
        {
            // create the entity and save to database
            var notificationEntity = new NotificationEntity
            {
                UserId = message.UserId,
                Title = message.Subject,
                Message = message.Body,
                ReferenceId = message.ReferenceId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            // get the repo and save the notification

            var repository = unitOfWork.GetRepository<NotificationEntity, Guid>();
            await repository.AddAsync(notificationEntity);
            await unitOfWork.SaveChangesAsync();

            // Loop through the requested types and deliver
            foreach (var type in types)
            {
                var strategy = _notificationStrategies.FirstOrDefault(s => s.Type == type);
                if (strategy != null)
                {
                    await strategy.DeliverAsync(message);
                }
            }
        }

        public async Task<PaginationResponse<NotificationDto>> GetUserNotificationsAsync(string userId, NotificationQueryParams queryParams)
        {
            var repo = unitOfWork.GetRepository<NotificationEntity, Guid>();

            var spec = new NotificationsByUserIdSpec(userId, queryParams);
            var countSpec = new NotificationsCountByUserIdSpec(userId, queryParams);

            var notifications = await repo.GetAllWithSpecAsync(spec);
            var totalCount = await repo.GetCountAsync(countSpec);

            var mappedData = mapper.Map<IReadOnlyList<NotificationDto>>(notifications);

            return new PaginationResponse<NotificationDto>(
                queryParams.PageIndex,
                queryParams.PageSize,
                totalCount,
                mappedData);
        }

        public async Task<bool> MarkAllAsReadAsync(string userId)
        {
            var repo = unitOfWork.GetRepository<NotificationEntity, Guid>();

            var unreadSpec = new UnreadNotificationsSpec(userId);
            var unreadNotifications = await repo.GetAllWithSpecAsync(unreadSpec);

            if (unreadNotifications == null || !unreadNotifications.Any())
                return true;

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                repo.Update(notification);
            }

            return await unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> MarkAsReadAsync(Guid notificationId, string userId)
        {
            var repo = unitOfWork.GetRepository<NotificationEntity, Guid>();
            var notification = await repo.GetByIdAsync(notificationId);

            if (notification == null)
                throw new NotificationNotFound("Notification not found.");

            if (notification.UserId != userId)
                throw new UnauthorizedExceptionCusotme();

            if (notification.IsRead)
                return true;

            notification.IsRead = true;
            repo.Update(notification);

            return await unitOfWork.SaveChangesAsync() > 0;
        }

    }
}
