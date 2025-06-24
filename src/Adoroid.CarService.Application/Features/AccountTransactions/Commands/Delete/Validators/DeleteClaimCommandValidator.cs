using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Commands.Delete.Validators;

public class DeleteClaimCommandValidator : AbstractValidator<DeleteClaimCommand>
{
    public DeleteClaimCommandValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}


