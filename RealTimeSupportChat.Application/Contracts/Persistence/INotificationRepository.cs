using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Application.Contracts.Persistence
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IReadOnlyList<Notification>> GetAllByReceiverIdAsync(string receiverId);
        Task<IReadOnlyList<Notification>> GetUnreadByReceiverIdAsync(string receiverId);
        Task CreateRangeAsync(IEnumerable<Notification> notifications);
        Task<List<Notification>> GetAllByIdsAsync(List<int> ids);
    }
}
