using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Employees.Queries.GetList.Validators;

public class GetEmployeeListQueryValidator : AbstractValidator<GetEmployeeListQuery>
{
    public GetEmployeeListQueryValidator()
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
