using System.ComponentModel.DataAnnotations;

namespace RealTimeSupportChat.Application.DTOs.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
