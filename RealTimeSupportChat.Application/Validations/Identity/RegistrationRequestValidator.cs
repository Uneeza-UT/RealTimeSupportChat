using FluentValidation;
using RealTimeSupportChat.Application.DTOs.Identity;

namespace RealTimeSupportChat.Application.Validations.Identity
{
    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(x => x.FirstName)
           .NotEmpty().WithMessage("{PropertyName} is required.");


            RuleFor(x => x.LastName)
               .NotEmpty().WithMessage("{PropertyName} is required.");


            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^[^@\s]+@[^@\s.]+(\.[^@\s.]+)+$").WithMessage("{PropertyName} must be a valid email address.");


            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(6).WithMessage("{PropertyName} must be atleast 6 characters.");


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} must be atleast 8 characters.");



            RuleFor(x => x.Role)
                .Must(role => role == "Customer" || role == "SupportManager" || role == "SupportAgent")
                .WithMessage("{PropertyName} must be either Customer, SupportManager or SupportAgent.");
        }
    }
}
