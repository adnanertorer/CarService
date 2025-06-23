using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Queries.GetById.Validators;

public class GetByIdCompanyServiceQueryValidator : AbstractValidator<GetByIdCompanyServiceQuery>
{
    public GetByIdCompanyServiceQueryValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
