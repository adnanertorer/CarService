using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Delete.Validators;

public class DeleteSubServiceCommandValidator : AbstractValidator<DeleteSubServiceCommand>
{
    public DeleteSubServiceCommandValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}