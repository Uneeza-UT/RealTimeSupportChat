using RealTimeSupportChat.Domain.Common;

namespace RealTimeSupportChat.Domain
{
    public class Notification : BaseEntity
    {
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
