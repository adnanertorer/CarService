using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Vehicles.Queries.GetList.Validators;

public class GetMyVehicleListQueryValidator : AbstractValidator<GetMyVehicleListQuery>
{
    public GetMyVehicleListQueryValidator()
    {
        RuleFor(i => i.PageRequest).NotNull()
      .WithMessage(ValidationMessages.PageRequestRequired);

        RuleFor(i => i.PageRequest.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeGreaterThanZero)
            .LessThan(100)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeLessThan100);
    }
}
