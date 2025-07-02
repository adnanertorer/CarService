using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Commands.Delete.Validators;

public class DeleteUserToCompanyCommandValidator : AbstractValidator<DeleteUserToCompanyCommand>
{
    public DeleteUserToCompanyCommandValidator()
    {
        RuleFor(x => x.Id)
           .NotNull()
           .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
