using FluentValidation;

using Meets.Identity.Users;

namespace Meets.Identity.RegisterEndpoints;

internal sealed class CreateUserInputValidator :
    AbstractValidator<CreateUserInput>
{
    public CreateUserInputValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}