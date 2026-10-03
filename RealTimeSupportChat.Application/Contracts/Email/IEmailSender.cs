using RealTimeSupportChat.Application.Models.Email;

namespace RealTimeSupportChat.Application.Contracts.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailMessageData emailMessageData);
    }
}
