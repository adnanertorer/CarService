using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Bookings.Qeries.GetByCompanyId.Validators;

public class GetByCompanyIdAsyncQueryValidator : AbstractValidator<GetByCompanyIdAsyncQuery>
{
    public GetByCompanyIdAsyncQueryValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Firma Id"))
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.NotNull, "Firma Id"));

        RuleFor(i => i.PageRequest).NotNull()
           .WithMessage(ValidationMessages.PageRequestRequired);

        RuleFor(i => i.PageRequest.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeGreaterThanZero)
            .LessThan(100)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeLessThan100);
    }
}
