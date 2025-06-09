using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Users.Queries.Login.Validators;

public class UserLoginQueryValidator : AbstractValidator<UserLoginQuery>
{
    public UserLoginQueryValidator()
    {
        RuleFor(x => x.Email)
               .NotEmpty()
               .WithMessage(string.Format(ValidationMessages.Required, "E-Posta"))
               .EmailAddress()
               .WithMessage(string.Format(ValidationMessages.Email, "E-Posta"));
    }
}
