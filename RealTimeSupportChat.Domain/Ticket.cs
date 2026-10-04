using RealTimeSupportChat.Domain.Common;
using RealTimeSupportChat.Domain.Enums;

namespace RealTimeSupportChat.Domain
{
    public class Ticket : BaseEntity
    {
        public string CustomerId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }       
        public string? AssignedToId { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
