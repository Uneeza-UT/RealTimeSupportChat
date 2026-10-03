using RealTimeSupportChat.Domain.Common;

namespace RealTimeSupportChat.Domain
{
    public class Message : BaseEntity
    {
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public string SenderId { get; set; }
        public string? Content { get; set; }

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
