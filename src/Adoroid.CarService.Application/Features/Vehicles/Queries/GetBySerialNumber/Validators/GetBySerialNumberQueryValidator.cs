using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Vehicles.Queries.GetBySerialNumber.Validators;

public class GetBySerialNumberQueryValidator : AbstractValidator<GetBySerialNumberQuery>
{
    public GetBySerialNumberQueryValidator()
    {
        RuleFor(i => i.SerialNumber)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Şase No"));

        RuleFor(i => i.PlateNumber)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Plaka"));
    }
}
