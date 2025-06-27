using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetList.Validators;

public class GetListMySubServiceQueryValidator : AbstractValidator<GetListMySubServiceQuery>
{
    public GetListMySubServiceQueryValidator()
    {
        RuleFor(i => i.PageRequest).NotNull()
           .WithMessage(ValidationMessages.PageRequestRequired);

        RuleFor(i => i.PageRequest.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeGreaterThanZero)
            .LessThan(100)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeLessThan100);


        RuleFor(x => x.MainServiceId)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "Service Id"));
    }
}
