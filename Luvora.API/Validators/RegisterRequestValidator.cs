using FluentValidation;
using Luvora.Application.DTOs.Auth;

namespace Luvora.API.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestValidator() 
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.");
            //.Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            //.Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
        }
    }
}
