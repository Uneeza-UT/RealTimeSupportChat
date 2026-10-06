using Microsoft.AspNetCore.SignalR;
using RealTimeSupportChat.Application.Contracts.Services;
using RealTimeSupportChat.Application.DTOs.Message;
using RealTimeSupportChat.Application.DTOs.Notification;
using RealTimeSupportChat.Infrastructure.Hubs;

namespace RealTimeSupportChat.Infrastructure.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IHubContext<SupportChatHub> _hubContext;

        public ChatHubService(IHubContext<SupportChatHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public async Task SendMessageAsync(string receiverId, GetMessageDto messageDto)
        {
            await _hubContext.Clients
                .Users(receiverId, messageDto.SenderId)
                .SendAsync("ReceiveMessage", messageDto);
        }



        public async Task DeleteMessagesAsync(List<int> messageIds, string senderId, string receiverId)
        {
            await _hubContext.Clients
                .Users(senderId, receiverId)
                .SendAsync("MessagesDeleted", messageIds);
        }



        public async Task SendNotificationAsync(GetNotificationDto notificationDto)
        {
            await _hubContext.Clients
                .User(notificationDto.ReceiverId)
                .SendAsync("ReceiveNotification", notificationDto);
        }


        public async Task SendNotificationsAsync(List<GetNotificationDto> notificationDtos)
        {
            foreach (var notificationDto in notificationDtos)
            {
                await _hubContext.Clients
                .User(notificationDto.ReceiverId)
                .SendAsync("ReceiveNotification", notificationDto);
            }         
        }
    }
}
