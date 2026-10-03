using RealTimeSupportChat.Domain.Enums;

namespace RealTimeSupportChat.Application.DTOs.Ticket
{
    public class ChangeTicketStatusDto
    {
        public int Id { get; set; }
        public TicketStatus Status { get; set; }
    }
}
