using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MainServices.Queries.GetById.Validators;

public class GetEntityByIdMainServiceQueryValidator : AbstractValidator<GetByIdMainServiceQuery>
{
    public GetEntityByIdMainServiceQueryValidator()
    {
        RuleFor(x => x.Id)
        .NotNull()
        .WithMessage(string.Format(ValidationMessages.Required, "Id bilgisi"));
    }
}
