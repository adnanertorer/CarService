using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Create.Validators;

public class CreateSubServiceCommandValidator : AbstractValidator<CreateSubServiceCommand>
{
    public CreateSubServiceCommandValidator()
    {

        RuleFor(x => x.MainServiceId)
           .NotNull()
           .WithMessage(string.Format(ValidationMessages.Required, "Service Id"));

        RuleFor(x => x.Operation)
          .NotEmpty()
          .WithMessage(string.Format(ValidationMessages.Required, "Yapılan iş"))
          .MaximumLength(150)
          .WithMessage(string.Format(ValidationMessages.MaxLength, "Yapılan iş", "150"));

        RuleFor(x => x.EmployeeId)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Personel Id"));

        RuleFor(x => x.OperationDate)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "İşlem zamanı"));

        RuleFor(x => x.Cost)
        .NotNull()
        .WithMessage(string.Format(ValidationMessages.Required, "Ücret"));
    }
}
