using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Bookings.Qeries.GetById.Validators;

public class BookingGetByIdQueryValidator : AbstractValidator<BookingGetByIdQuery>
{
    public BookingGetByIdQueryValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Randevu Id"))
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.NotNull, "Randevu Id"));
    }
}
