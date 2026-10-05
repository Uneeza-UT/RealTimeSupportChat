using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RealTimeSupportChat.Infrastructure.Hubs
{
    [Authorize]
    public class SupportChatHub : Hub
    {

    }
}
