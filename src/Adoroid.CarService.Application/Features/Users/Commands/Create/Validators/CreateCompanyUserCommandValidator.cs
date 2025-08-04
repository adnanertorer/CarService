using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Users.Commands.Create.Validators;

public class CreateCompanyUserCommandValidator : AbstractValidator<CreateCompanyUserCommand>
{
    public CreateCompanyUserCommandValidator()
    {
        RuleFor(x => x.CreateUserDto.Email)
              .NotEmpty()
              .WithMessage(string.Format(ValidationMessages.Required, "E-Posta"))
              .EmailAddress()
              .WithMessage(string.Format(ValidationMessages.Email, "E-Posta"));

        RuleFor(x => x.CreateUserDto.Name)
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.Required, "Adı"))
             .MaximumLength(50)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Adı", "50"));

        RuleFor(x => x.CreateUserDto.Surname)
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.Required, "Soyadı"))
             .MaximumLength(50)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Soyadı", "50"));

        RuleFor(x => x.CreateUserDto.Password)
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.Required, "Parola"))
             .MinimumLength(8)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Parola", "8"));

        RuleFor(x => x.CreateUserDto.PhoneNumber)
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.Required, "Telefon"))
             .MinimumLength(10)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Telefon", "10"))
             .MaximumLength(16)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Telefon", "16"));


        RuleFor(x => x.CreateCompanyDto.CompanyEmail)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "E-Posta"))
            .EmailAddress()
            .WithMessage(string.Format(ValidationMessages.Email, "E-Posta"))
            .MaximumLength(60)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "E-Posta", "60"));

        RuleFor(x => x.CreateCompanyDto.CompanyName)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Firma adı"))
            .MaximumLength(150)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Firma adı", "150"));

        RuleFor(x => x.CreateCompanyDto.AuthorizedName)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Yetkili adı"))
            .MaximumLength(50)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Yetkili adı", "50"));

        RuleFor(x => x.CreateCompanyDto.AuthorizedSurname)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Yetkili soyadı"))
            .MaximumLength(50)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Yetkili soyadı", "50"));

        RuleFor(x => x.CreateCompanyDto.TaxNumber)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Vergi numarası"))
            .MaximumLength(16)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Vergi numarası", "16"))
            .MinimumLength(10)
            .WithMessage(string.Format(ValidationMessages.MinLength, "Vergi numarası", "10"));

        RuleFor(x => x.CreateCompanyDto.TaxOffice)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Vergi dairesi"))
            .MaximumLength(150)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Vergi dairesi", "150"));

        RuleFor(x => x.CreateCompanyDto.CompanyAddress)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Adres"))
            .MaximumLength(250)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Adres", "250"));

        RuleFor(x => x.CreateCompanyDto.CompanyPhone)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Telefon"))
            .MaximumLength(16)
            .WithMessage(string.Format(ValidationMessages.MaxLength, "Telefon", "16"));

        RuleFor(x => x.CreateCompanyDto.CityId)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.GreaterThanZero, "İl"));

        RuleFor(x => x.CreateCompanyDto.DistrictId)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.GreaterThanZero, "İlçe"));
    }
}
