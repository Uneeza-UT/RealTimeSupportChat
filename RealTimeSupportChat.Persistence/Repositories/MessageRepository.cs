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
    }
}
