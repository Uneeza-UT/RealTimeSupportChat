using RealTimeSupportChat.Application.DTOs.Message;

namespace RealTimeSupportChat.Application.Contracts.Services
{
    public interface IMessageService
    {
        Task<List<GetMessageDto>> GetByTicketIdAsync(int ticketId);
        Task<int> SendAsync(CreateMessageDto dto);
        Task DeleteRangeAsync(List<int> ids);
    }
}
