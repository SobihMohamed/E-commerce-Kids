using AutoMapper;
using E_commerce.Domain.Models.Notification;
using E_commerce.Shared.Dto_s.Notificaiton;


namespace E_commerce.Services.AutoMapper.NotificationMapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationEntity, NotificationDto>();
        }
    }
}
