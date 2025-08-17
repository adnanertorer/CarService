using FluentValidation;

namespace Adoroid.CarService.Application.Features.Users.Commands.Update.Validators;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Eski şifre boş olamaz.")
            .MinimumLength(8).WithMessage("Eski şifre en az 8 karakter olmalıdır.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
            .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.")
            .NotEqual(x => x.OldPassword)
            .WithMessage("Yeni şifre eski şifre ile aynı olmamalıdır.");
    }
}
