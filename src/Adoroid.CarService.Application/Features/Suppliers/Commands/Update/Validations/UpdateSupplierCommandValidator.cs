using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Suppliers.Commands.Update.Validations;

public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Id"));

        RuleFor(x => x.Email)
           .NotEmpty()
           .WithMessage(string.Format(ValidationMessages.Required, "E-Posta"))
           .EmailAddress()
           .WithMessage(string.Format(ValidationMessages.Email, "E-Posta"))
           .MaximumLength(60)
           .WithMessage(string.Format(ValidationMessages.MaxLength, "60"));

        RuleFor(x => x.Address)
            .MaximumLength(250)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "250"));

        RuleFor(x => x.ContactName)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Yetkili adı"))
            .MaximumLength(50)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "50"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Tedarikçi adı"))
            .MaximumLength(150)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "16"));

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Telefon"))
            .MaximumLength(20)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "20"));
    }
}
