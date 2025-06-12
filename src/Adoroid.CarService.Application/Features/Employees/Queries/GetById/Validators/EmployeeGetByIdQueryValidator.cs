using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Employees.Queries.GetById.Validators;

public class EmployeeGetByIdQueryValidator : AbstractValidator<EmployeeGetByIdQuery>
{
    public EmployeeGetByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotNull()
           .WithMessage(string.Format(ValidationMessages.NotNull, "Id"));
    }
}
