namespace RealTimeSupportChat.Application.Contracts.Identity
{
    public interface ICurrentUserService
    {
        public string UserId { get; }
        public string Role { get; }
    }
}
