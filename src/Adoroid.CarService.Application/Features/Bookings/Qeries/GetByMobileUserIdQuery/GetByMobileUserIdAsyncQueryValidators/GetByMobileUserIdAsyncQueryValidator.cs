using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Bookings.Qeries.GetByMobileUserIdQuery.GetByMobileUserIdAsyncQueryValidators;

public class GetByMobileUserIdAsyncQueryValidator : AbstractValidator<GetByMobileUserIdAsyncQuery>
{
    public GetByMobileUserIdAsyncQueryValidator()
    {
        RuleFor(x => x.MobileUserId)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "User Id"))
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.NotNull, "User Id"));

        RuleFor(i => i.PageRequest).NotNull()
           .WithMessage(ValidationMessages.PageRequestRequired);

        RuleFor(i => i.PageRequest.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeGreaterThanZero)
            .LessThan(100)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeLessThan100);
    }
}
