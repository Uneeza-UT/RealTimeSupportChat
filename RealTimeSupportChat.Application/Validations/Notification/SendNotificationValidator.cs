using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Notification;

namespace RealTimeSupportChat.Application.Validations.Notification
{
    public class SendNotificationValidator : AbstractValidator<SendNotificationDto>
    {
        public SendNotificationValidator()
        {
            RuleFor(t => t.TicketId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");


            RuleFor(t => t.ReceiverId)
                .NotEmpty().WithMessage("{PropertyName} is required.");


            RuleFor(t => t.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(500).WithMessage("{PropertyName} cannot exceed 500 characters");
        }
    }
}
