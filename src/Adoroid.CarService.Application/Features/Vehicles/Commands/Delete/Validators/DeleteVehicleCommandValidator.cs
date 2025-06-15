using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Vehicles.Commands.Delete.Validators;

public class DeleteVehicleCommandValidator : AbstractValidator<DeleteVehicleCommand>
{
    public DeleteVehicleCommandValidator()
    {
        RuleFor(x => x.Id)
        .NotNull()
        .WithMessage(string.Format(ValidationMessages.NotNull, "Id"));
    }
}
