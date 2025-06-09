using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Users.Commands.Create.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
               .NotEmpty()
               .WithMessage(string.Format(ValidationMessages.Required, "E-Posta"))
               .EmailAddress()
               .WithMessage(string.Format(ValidationMessages.Email, "E-Posta"));

        RuleFor(x => x.CompanyId).NotNull().WithMessage(string.Format(ValidationMessages.Required, "Firma"));

        RuleFor(x => x.Name)
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.Required, "Adı"))
             .MaximumLength(50)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Adı", "50"));

        RuleFor(x => x.Surname)
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.Required, "Soyadı"))
             .MaximumLength(50)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Soyadı", "50"));

        RuleFor(x => x.Password)
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.Required, "Parola"))
             .MinimumLength(8)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Parola", "8"));

        RuleFor(x => x.PhoneNumber)
             .NotEmpty()
             .WithMessage(string.Format(ValidationMessages.Required, "Telefon"))
             .MinimumLength(10)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Telefon", "10"))
             .MaximumLength(16)
             .WithMessage(string.Format(ValidationMessages.MaxLength, "Telefon", "16"));
    }
}
