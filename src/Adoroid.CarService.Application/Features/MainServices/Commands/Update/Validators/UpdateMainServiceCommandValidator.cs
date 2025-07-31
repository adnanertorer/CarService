using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Update.Validators;

public class UpdateMainServiceCommandValidator : AbstractValidator<UpdateMainServiceCommand>
{
    public UpdateMainServiceCommandValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id bilgisi"));

        RuleFor(x => x.VehicleId)
           .NotNull()
           .WithMessage(string.Format(ValidationMessages.Required, "Araç bilgisi"));

        RuleFor(x => x.Description)
           .MaximumLength(250)
           .WithMessage(string.Format(ValidationMessages.MaxLength, "Açıklama", "250"))
           .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
