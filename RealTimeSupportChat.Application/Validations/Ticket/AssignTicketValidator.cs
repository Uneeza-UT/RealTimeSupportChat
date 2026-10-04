using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Ticket;

namespace RealTimeSupportChat.Application.Validations.Ticket
{
    public class AssignTicketValidator : AbstractValidator<AssignTicketDto>
    {
        public AssignTicketValidator()
        {
            RuleFor(t => t.Id)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");


            RuleFor(t => t.AssignedToId)
               .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
