using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Ticket;

namespace RealTimeSupportChat.Application.Validations.Ticket
{
    public class ChangeTicketStatusValidator : AbstractValidator<ChangeTicketStatusDto>
    {
        public ChangeTicketStatusValidator()
        {
            RuleFor(t => t.Status)
                .IsInEnum().WithMessage("Allowed values: Open, InProgress, Resolved, Closed.");
        }
    }
}
