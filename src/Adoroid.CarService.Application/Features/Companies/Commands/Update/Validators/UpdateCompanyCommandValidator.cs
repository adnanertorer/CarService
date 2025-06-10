using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Companies.Commands.Update.Validators;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.Id)
           .NotNull()
           .WithMessage(string.Format(ValidationMessages.Required, "Id"));

        RuleFor(x => x.CompanyEmail)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "E-Posta"))
            .EmailAddress()
            .WithMessage(string.Format(ValidationMessages.Email, "E-Posta"))
            .MaximumLength(60)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "60"));

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Firma adı"))
            .MaximumLength(150)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "150"));

        RuleFor(x => x.AuthorizedName)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Yetkili adı"))
            .MaximumLength(50)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "50"));

        RuleFor(x => x.AuthorizedSurname)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Yetkili soyadı"))
            .MaximumLength(50)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "50"));

        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Vergi numarası"))
            .MaximumLength(16)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "16"))
            .MinimumLength(10)
            .WithMessage(string.Format(ValidationMessages.MinLength, "10"));

        RuleFor(x => x.TaxOffice)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Vergi dairesi"))
            .MaximumLength(150)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "150"));

        RuleFor(x => x.CompanyAddress)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Adres"))
            .MaximumLength(250)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "250"));

        RuleFor(x => x.CompanyPhone)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Telefon"))
            .MaximumLength(16)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "16"));

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.GreaterThanZero, "İl"));

        RuleFor(x => x.DistrictId)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.GreaterThanZero, "İlçe"));
    }
}
