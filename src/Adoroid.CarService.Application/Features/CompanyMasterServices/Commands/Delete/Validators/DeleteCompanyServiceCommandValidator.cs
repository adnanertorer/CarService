using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Delete.Validators;

public class DeleteCompanyServiceCommandValidator : AbstractValidator<DeleteCompanyServiceCommand>
{
    public DeleteCompanyServiceCommandValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
