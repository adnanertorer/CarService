using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Create.Validators;

public class CreateCompanyServiceCommandValidator : AbstractValidator<CreateCompanyServiceCommand>
{
    public CreateCompanyServiceCommandValidator()
    {
        RuleFor(x => x.MasterServiceId)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Hizmet id"));
    }
}
