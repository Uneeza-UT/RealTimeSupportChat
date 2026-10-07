using Microsoft.AspNetCore.SignalR;

namespace RealTimeSupportChat.Infrastructure.Services
{
    public class SignalRUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("uid")?.Value;
        }
    }
}
