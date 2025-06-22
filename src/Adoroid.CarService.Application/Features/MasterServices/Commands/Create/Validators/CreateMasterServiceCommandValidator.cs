using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MasterServices.Commands.Create.Validators;

public class CreateMasterServiceCommandValidator : AbstractValidator<CreateMasterServiceCommand>
{
    public CreateMasterServiceCommandValidator()
    {
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
