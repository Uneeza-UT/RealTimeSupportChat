using RealTimeSupportChat.Application.DTOs.Attachment;

namespace RealTimeSupportChat.Application.DTOs.Message
{
    public class GetMessageDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string SenderId { get; set; }
        public string? Content { get; set; }

        public List<GetAttachmentDto> Attachments { get; set; } = new List<GetAttachmentDto>();
    }
}
