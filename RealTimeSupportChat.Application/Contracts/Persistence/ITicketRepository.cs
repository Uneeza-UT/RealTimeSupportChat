using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Application.Contracts.Persistence
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {   
        Task<IReadOnlyList<Ticket>> GetAllByCustomerIdAsync(string customerId);
        Task<IReadOnlyList<Ticket>> GetAllByAssignedToIdAsync(string assignedToId);
    }
}
