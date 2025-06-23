using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Commands.Create.Validators;

public class CreateClaimCommandValidator : AbstractValidator<CreateClaimCommand>
{
    public CreateClaimCommandValidator()
    {
        RuleFor(x => x.CustomerId)
           .NotNull()
           .WithMessage(string.Format(ValidationMessages.Required, "Müşteri Id"));

        RuleFor(x => x.TransactionDate)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "İşlem Tarihi"))
          .GreaterThan(DateTime.UtcNow)
          .WithMessage(string.Format(ValidationMessages.GreaterThanNow, "İşlem Tarihi");

        RuleFor(x => x.Claim)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "Miktar"))
         .GreaterThan(0)
         .WithMessage(string.Format(ValidationMessages.Required, "Miktar"));
    }
}
