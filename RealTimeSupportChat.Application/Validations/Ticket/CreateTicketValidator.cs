using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Ticket;

namespace RealTimeSupportChat.Application.Validations.Ticket
{
    public class CreateTicketValidator : AbstractValidator<CreateTicketDto>
    {
        public CreateTicketValidator()
        {
            RuleFor(t => t.Subject)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");


            RuleFor(t => t.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.");

        }
    }
}
