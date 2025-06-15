using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Vehicles.Commands.Create.Validators;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.CustomerId)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.NotNull, "CustomerId"));

        RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Marka adı"))
            .MaximumLength(250)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Marka adı", "50"));

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Model adı"))
            .MaximumLength(50)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Model adı", "50"));

        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Plaka"))
            .MaximumLength(20)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Plaka", "20"));

        RuleFor(x => x.Engine)
            .MaximumLength(20)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Motor", "20"))
            .When(i => !string.IsNullOrWhiteSpace(i.Engine));

        RuleFor(x => x.SerialNumber)
           .MaximumLength(20)
           .WithMessage(string.Format(ValidationMessages.MaxLength, "Seri numarası", "20"))
           .When(i => !string.IsNullOrWhiteSpace(i.Engine));

        RuleFor(x => x.FuelTypeId)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.GreaterThanZero, "Yakıt Tipi"));

        RuleFor(x => x.Year)
           .GreaterThan(0)
           .WithMessage(string.Format(ValidationMessages.GreaterThanZero, "Yıl"));

    }
}
