using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Application.Contracts.Persistence
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {   
        Task<IReadOnlyList<Ticket>> GetAllByCustomerIdAsync(string customerId);
        Task<IReadOnlyList<Ticket>> GetAllByAssignedToIdAsync(string assignedToId);
        Task<Ticket?> GetByIdAndCustomerIdAsync(int ticketId, string customerId);
        Task<Ticket?> GetByIdAndAssignedToIdAsync(int ticketId, string assignedToId);
    }
}
