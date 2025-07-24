using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Suppliers.Commands.Create.Validations;

public class CreateSupplierCommandValidation : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidation()
    {
        RuleFor(x => x.Email)
           .EmailAddress()
           .WithMessage(string.Format(ValidationMessages.Email, "E-Posta")).When(x => !string.IsNullOrWhiteSpace(x.Email))
           .MaximumLength(60)
           .WithMessage(string.Format(ValidationMessages.MaxLength, "E-Posta", "60")).When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Address)
            .MaximumLength(250)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Adres", "250"));

        RuleFor(x => x.ContactName)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Yetkili adı"))
            .MaximumLength(50)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Yetkili adı", "50"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Tedarikçi adı"))
            .MaximumLength(150)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Tedarikçi adı", "250"));

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Telefon"))
            .MaximumLength(20)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Telefon", "20"));
    }
}
