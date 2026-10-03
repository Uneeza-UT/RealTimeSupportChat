using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Application.Contracts.Persistence
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<IReadOnlyList<Ticket>> GetAsync();
    }
}
