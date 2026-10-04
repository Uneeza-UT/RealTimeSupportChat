using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Message;

namespace RealTimeSupportChat.Application.Validations.Message
{
    public class CreateMessageValidator : AbstractValidator<CreateMessageDto>
    {
        public CreateMessageValidator()
        {
            RuleFor(m => m.TicketId)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");


            RuleFor(m => m.Content)
               .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
