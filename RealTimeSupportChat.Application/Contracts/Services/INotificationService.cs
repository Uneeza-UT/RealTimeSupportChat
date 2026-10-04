using RealTimeSupportChat.Application.DTOs.Notification;

namespace RealTimeSupportChat.Application.Contracts.Services
{
    public interface INotificationService
    {
        Task<List<GetNotificationDto>> GetAsync();
        Task<List<GetNotificationDto>> GetUnreadAsync();
        Task SendAsync(SendNotificationDto dto);
        Task SendManyAsync(List<SendNotificationDto> dtos);
        Task MarkAsReadAsync(MarkNotificationAsReadDto dto);
    }
}
