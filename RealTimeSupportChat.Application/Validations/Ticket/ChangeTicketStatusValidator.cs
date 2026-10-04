using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Ticket;

namespace RealTimeSupportChat.Application.Validations.Ticket
{
    public class ChangeTicketStatusValidator : AbstractValidator<ChangeTicketStatusDto>
    {
        public ChangeTicketStatusValidator()
        {
            RuleFor(t => t.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");


            RuleFor(t => t.Status)
                .IsInEnum().WithMessage("Allowed values: Open, InProgress, Resolved, Closed.");
        }
    }
}
