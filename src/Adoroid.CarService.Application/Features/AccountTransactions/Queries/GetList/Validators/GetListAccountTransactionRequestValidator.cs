using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetList.Validators;

public class GetListAccountTransactionRequestValidator : AbstractValidator<GetListAccountTransactionRequest>
{
    public GetListAccountTransactionRequestValidator()
    {
        RuleFor(i => i.MainFilterRequest.PageRequest).NotNull()
           .WithMessage(ValidationMessages.PageRequestRequired);

        RuleFor(i => i.MainFilterRequest.PageRequest.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeGreaterThanZero)
            .LessThan(100)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeLessThan100);

        RuleFor(i => i.MainFilterRequest.EndDate)
            .GreaterThan(i => i.MainFilterRequest.StartDate)
            .WithMessage(ValidationMessages.StartDateMustBeLessThanEndDate)
            .When(i => i.MainFilterRequest.EndDate.HasValue && i.MainFilterRequest.StartDate.HasValue);
    }
}
