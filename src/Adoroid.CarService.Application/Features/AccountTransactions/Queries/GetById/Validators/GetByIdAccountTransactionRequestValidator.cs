using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetById.Validators;

public class GetByIdAccountTransactionRequestValidator : AbstractValidator<GetByIdAccountTransactionRequest>
{
    public GetByIdAccountTransactionRequestValidator()
    {
        RuleFor(x => x.Id)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
