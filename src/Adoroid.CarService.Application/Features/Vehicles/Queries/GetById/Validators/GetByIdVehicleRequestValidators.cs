using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Vehicles.Queries.GetById.Validators;

public class GetByIdVehicleRequestValidators : AbstractValidator<GetByIdVehicleRequest>
{
    public GetByIdVehicleRequestValidators()
    {
        RuleFor(x => x.Id)
       .NotNull()
       .WithMessage(string.Format(ValidationMessages.NotNull, "Id"));
    }
}
