using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Notification;

namespace RealTimeSupportChat.Application.Validations.Notification
{
    public class MarkNotificationAsReadValidator : AbstractValidator<MarkNotificationAsReadDto>
    {
        public MarkNotificationAsReadValidator()
        {
            RuleFor(n => n.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");


            RuleFor(n => n.IsRead)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
