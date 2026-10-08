using Microsoft.EntityFrameworkCore;
using RealTimeSupportChat.Application.Contracts.Persistence;
using RealTimeSupportChat.Domain;
using RealTimeSupportChat.Persistence.DatabaseContext;

namespace RealTimeSupportChat.Persistence.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }


        // Get all messages belonging to a specific ticket if the user is the customer or assigned support agent
        public async Task<IReadOnlyList<Message>> GetAllByTicketIdAndUserIdAsync(int ticketId, string userId)
        {
            return await _dbContext.Set<Message>()
                .AsNoTracking()
                .Include(m => m.Attachments)
                .Include(m => m.Ticket)
                .Where(m => m.TicketId == ticketId && 
                    (m.Ticket.CustomerId == userId || m.Ticket.AssignedToId == userId))
                .ToListAsync();
        }




        // Get a list of message entities with attachments
        public async Task<List<Message>> GetAllByIdsWithAttachmentsAsync(List<int> ids)
        {
            return await _dbContext.Set<Message>()
                .AsNoTracking()
                .Include(m => m.Attachments)
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();
        }
    }
}
