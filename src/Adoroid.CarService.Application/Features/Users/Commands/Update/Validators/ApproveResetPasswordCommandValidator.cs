using FluentValidation;

namespace Adoroid.CarService.Application.Features.Users.Commands.Update.Validators;

public class ApproveResetPasswordCommandValidator : AbstractValidator<ApproveResetPasswordCommand>
{
    public ApproveResetPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
            .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.");
    }
}
