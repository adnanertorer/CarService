using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MobileUsers.Queries.Login.Validators;

public class MobileUserLoginQueryValidator : AbstractValidator<MobileUserLoginQuery>
{
    public MobileUserLoginQueryValidator()
    {
        RuleFor(x => x.Email)
               .NotEmpty()
               .WithMessage(string.Format(ValidationMessages.Required, "E-Posta"))
               .EmailAddress()
               .WithMessage(string.Format(ValidationMessages.Email, "E-Posta"));

        RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage(string.Format(ValidationMessages.Required, "Parola"));
    }
}
