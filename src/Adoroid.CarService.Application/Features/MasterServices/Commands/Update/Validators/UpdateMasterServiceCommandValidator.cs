using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MasterServices.Commands.Update.Validators;

public class UpdateMasterServiceCommandValidator : AbstractValidator<UpdateMasterServiceCommand>
{
    public UpdateMasterServiceCommandValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id"));

        RuleFor(x => x.ServiceName)
           .NotEmpty()
           .WithMessage(string.Format(ValidationMessages.Required, "Servis adı"))
           .MaximumLength(150)
           .WithMessage(string.Format(ValidationMessages.MaxLength, "Servis adı", "150"));

        RuleFor(x => x.OrderIndex)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Sıralama"));
    }
}
