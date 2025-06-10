using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Companies.Queries.GetById.Validators;

public class CompanyGetByIdQueryValidator : AbstractValidator<CompanyGetByIdQuery>
{
    public CompanyGetByIdQueryValidator()
    {
        RuleFor(x => x.Id)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
