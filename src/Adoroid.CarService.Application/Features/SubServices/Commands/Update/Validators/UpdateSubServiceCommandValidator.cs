using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Update.Validators;

public class UpdateSubServiceCommandValidator : AbstractValidator<UpdateSubServiceCommand>
{
    public UpdateSubServiceCommandValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id"));

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
