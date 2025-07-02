using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Commands.Create.Validators;

public class CreateUserToCompanyCommandValidator : AbstractValidator<CreateUserToCompanyCommand>
{
    public CreateUserToCompanyCommandValidator()
    {
        RuleFor(x => x.CompanyUserType)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Kullanıcı tipi"));

        RuleFor(x => x.CompanyId)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Firma Id"));
    }
}
