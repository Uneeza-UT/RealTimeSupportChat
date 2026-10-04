using Microsoft.EntityFrameworkCore;
using RealTimeSupportChat.Application.Contracts.Persistence;
using RealTimeSupportChat.Domain;
using RealTimeSupportChat.Persistence.DatabaseContext;

namespace RealTimeSupportChat.Persistence.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        // Get all tickets that belong to a specific customer
        public async Task<IReadOnlyList<Ticket>> GetAllByCustomerIdAsync(string customerId)
        {
            return await _dbContext.Set<Ticket>()
                .AsNoTracking()
                .Where(t => t.CustomerId == customerId)
                .ToListAsync();
        }


        // Get all tickets that are assigned to a specific support agent
        public async Task<IReadOnlyList<Ticket>> GetAllByAssignedToIdAsync(string assignedToId)
        {
            return await _dbContext.Set<Ticket>()
                .AsNoTracking()
                .Where(t => t.AssignedToId == assignedToId)
                .ToListAsync();
        }


        // Get one ticket that belongs to a specific customer
        public async Task<Ticket?> GetByIdAndCustomerIdAsync(int ticketId, string customerId)
        {
            return await _dbContext.Set<Ticket>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == ticketId && t.CustomerId == customerId);
        }


        // Get one ticket that is assigned to a specific support agent
        public async Task<Ticket?> GetByIdAndAssignedToIdAsync(int ticketId, string assignedToId)
        {
            return await _dbContext.Set<Ticket>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == ticketId && t.AssignedToId == assignedToId);
        }
    }
}
