using RealTimeSupportChat.Domain.Enums;

namespace RealTimeSupportChat.Application.DTOs.Ticket
{
    public class GetTicketDto
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public string? AssignedToId { get; set; }
        public string? PreviousAssigneeId { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
