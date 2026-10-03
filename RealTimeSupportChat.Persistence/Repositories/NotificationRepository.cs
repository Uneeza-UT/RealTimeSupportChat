using RealTimeSupportChat.Application.Contracts.Persistence;
using RealTimeSupportChat.Domain;
using RealTimeSupportChat.Persistence.DatabaseContext;

namespace RealTimeSupportChat.Persistence.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
