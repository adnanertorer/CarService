using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Customers.Commands.Update.Validators;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotNull()
          .WithMessage(string.Format(ValidationMessages.NotNull, "Id"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Müşteri adı"))
            .MaximumLength(50)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Müşteri adı", "50"));

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Müşteri soyadı"))
            .MaximumLength(50)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Müşteri adı", "50"));

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Telefon"))
            .MaximumLength(20)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Telefon", "20"));

        RuleFor(x => x.IsActive)
           .NotNull()
           .WithMessage(string.Format(ValidationMessages.Required, "Aktif/Pasif"));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(string.Format(ValidationMessages.Required, "E-Posta"))
            .MaximumLength(60)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "E-Posta", "60"))
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Address)
           .MaximumLength(250)
           .WithMessage(string.Format(ValidationMessages.MaxLength, "Adres", "250"))
           .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.TaxNumber)
          .MaximumLength(16)
          .WithMessage(string.Format(ValidationMessages.MaxLength, "Vergi Numarası", "16"))
          .When(x => !string.IsNullOrEmpty(x.TaxNumber));

        RuleFor(x => x.TaxOffice)
         .MaximumLength(150)
         .WithMessage(string.Format(ValidationMessages.MaxLength, "Vergi Dairesi", "150"))
         .When(x => !string.IsNullOrEmpty(x.TaxNumber));
    }
}
