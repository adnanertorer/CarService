using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MainServices.Queries.GetList.Validators;

public class GetListMainServiceQueryValidator : AbstractValidator<GetListMainServiceQuery>
{
    public GetListMainServiceQueryValidator()
    {
        RuleFor(i => i.FilterRequest.PageRequest).NotNull()
            .WithMessage(ValidationMessages.PageRequestRequired);

        RuleFor(i => i.FilterRequest.PageRequest.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeGreaterThanZero)
            .LessThan(100)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeLessThan100);

        RuleFor(i => i.FilterRequest.EndDate)
            .GreaterThan(i => i.FilterRequest.StartDate)
            .WithMessage(ValidationMessages.StartDateMustBeLessThanEndDate)
            .When(i => i.FilterRequest.EndDate.HasValue && i.FilterRequest.StartDate.HasValue);
    }
}
