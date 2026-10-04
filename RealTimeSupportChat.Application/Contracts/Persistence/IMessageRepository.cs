using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Application.Contracts.Persistence
{
    public interface IMessageRepository: IGenericRepository<Message>
    {     
        Task<IReadOnlyList<Message>> GetAllByTicketIdAsync(int ticketId);
        Task<IReadOnlyList<Message>> GetAllByTicketIdAndUserIdAsync(int ticketId, string userId);
        Task<List<Message>> GetAllByIdsWithAttachmentsAsync(List<int> ids);
    }
}
