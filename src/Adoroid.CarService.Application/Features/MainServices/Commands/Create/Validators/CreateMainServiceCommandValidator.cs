using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Create.Validators;

public class CreateMainServiceCommandValidator : AbstractValidator<CreateMainServiceCommand>
{
    public CreateMainServiceCommandValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Araç bilgisi"));

        RuleFor(x => x.ServiceDate)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Servis tarihi"))
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(string.Format(ValidationMessages.GreaterThanNow, "Servis tarihi"));

        RuleFor(x => x.Description)
           .MaximumLength(250)
           .WithMessage(string.Format(ValidationMessages.MaxLength, "Açıklama", "250"))
           .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
