using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Message;

namespace RealTimeSupportChat.Application.Validations.Message
{
    public class CreateMessageValidator : AbstractValidator<CreateMessageDto>
    {
        public CreateMessageValidator()
        {
            RuleFor(m => m)
                .Must(m =>
                    !string.IsNullOrWhiteSpace(m.Content) ||
                    m.Attachments.Any())
                .WithMessage("A message must contain text or at least one attachment.");

        }
    }
}
