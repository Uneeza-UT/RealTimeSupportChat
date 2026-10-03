using Microsoft.AspNetCore.Http;

namespace RealTimeSupportChat.Application.DTOs.Message
{
    public class CreateMessageDto
    {
        public int TicketId { get; set; }
        public string? Content { get; set; }
        public List<IFormFile>? Attachments { get; set; } 
    }
}
