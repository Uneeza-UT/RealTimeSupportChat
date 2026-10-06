using RealTimeSupportChat.Application.DTOs.Message;

namespace RealTimeSupportChat.Application.Contracts.Services
{
    public interface IMessageService
    {
        Task<List<GetMessageDto>> GetByTicketIdAsync(int ticketId);
        Task<int> SendAsync(int ticketId, CreateMessageDto dto);
        Task DeleteRangeAsync(int ticketId, List<int> ids);
    }
}
