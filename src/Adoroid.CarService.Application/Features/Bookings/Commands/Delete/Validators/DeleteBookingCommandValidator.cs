using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Bookings.Commands.Delete.Validators;

public class DeleteBookingCommandValidator : AbstractValidator<DeleteBookingCommand>
{
    public DeleteBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Randevu Id"))
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.NotNull, "Randevu Id"));
    }
}
