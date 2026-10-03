using AutoMapper;
using RealTimeSupportChat.Application.DTOs.Notification;
using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Application.MappingProfiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, GetNotificationDto>();
        }
    }
}
