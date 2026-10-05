using RealTimeSupportChat.Application.DTOs.Message;
using RealTimeSupportChat.Application.DTOs.Notification;

namespace RealTimeSupportChat.Application.Contracts.Services
{
    public interface IChatHubService
    {
        Task SendMessageAsync(string receiverId, GetMessageDto messageDto);
        Task DeleteMessagesAsync(List<int> messageIds, string senderId, string receiverId);
        Task SendNotificationAsync(GetNotificationDto notificationDto);
        Task SendNotificationsAsync(List<GetNotificationDto> notificationDtos);
    }
}
