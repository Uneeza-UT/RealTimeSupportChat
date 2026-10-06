using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Ticket;

namespace RealTimeSupportChat.Application.Validations.Ticket
{
    public class AssignTicketValidator : AbstractValidator<AssignTicketDto>
    {
        public AssignTicketValidator()
        {
            RuleFor(t => t.AssignedToId)
               .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
