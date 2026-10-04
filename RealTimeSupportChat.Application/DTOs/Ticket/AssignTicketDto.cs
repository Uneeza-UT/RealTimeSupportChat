using RealTimeSupportChat.Domain.Enums;

namespace RealTimeSupportChat.Application.DTOs.Ticket
{
    public class AssignTicketDto
    {
        public int Id { get; set; }
        public string AssignedToId { get; set; }
    }
}
