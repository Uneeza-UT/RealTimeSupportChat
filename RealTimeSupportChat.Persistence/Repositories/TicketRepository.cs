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

        public async Task<IReadOnlyList<Ticket>> GetAsync()
        {
            return await _dbContext.Set<Ticket>()
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
