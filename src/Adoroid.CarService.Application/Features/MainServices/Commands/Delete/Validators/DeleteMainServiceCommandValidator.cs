using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Delete.Validators;

public class DeleteMainServiceCommandValidator : AbstractValidator<DeleteMainServiceCommand>{
    public DeleteMainServiceCommandValidator()
    {
        RuleFor(x => x.Id)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "Id bilgisi"));
    }
}
