using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Queries.GetById.Validators;

public class GetByIdUserToCompanyQueryValidator : AbstractValidator<GetByIdUserToCompanyQuery>
{
    public GetByIdUserToCompanyQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
