using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Update.Validators;

public class UpdateCompanyServiceCommandValidator : AbstractValidator<UpdateCompanyServiceCommand>
{
    public UpdateCompanyServiceCommandValidator()
    {
        RuleFor(x => x.Id)
           .NotNull()
           .WithMessage(string.Format(ValidationMessages.Required, "Id"));

        RuleFor(x => x.MasterServiceId)
           .NotNull()
           .WithMessage(string.Format(ValidationMessages.Required, "Hizmet id"));
    }
}
