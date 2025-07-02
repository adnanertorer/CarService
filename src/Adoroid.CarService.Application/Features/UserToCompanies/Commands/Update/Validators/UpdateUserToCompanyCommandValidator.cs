using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Commands.Update.Validators;

public class UpdateUserToCompanyCommandValidator : AbstractValidator<UpdateUserToCompanyCommand>
{
    public UpdateUserToCompanyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Id"));

        RuleFor(x => x.CompanyUserType)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Kullanıcı tipi"));

        RuleFor(x => x.CompanyId)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Firma Id"));
    }
}
