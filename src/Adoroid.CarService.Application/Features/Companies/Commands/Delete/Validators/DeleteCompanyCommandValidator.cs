using Adoroid.CarService.Application.Features.Companies.ExceptionMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Companies.Commands.Delete.Validators;

public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
