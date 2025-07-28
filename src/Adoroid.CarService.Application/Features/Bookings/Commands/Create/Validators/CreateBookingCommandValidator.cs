using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Bookings.Commands.Create.Validators;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Firma Id"));

        RuleFor(x => x.BookingDate)
            .NotNull()
            .WithMessage(string.Format(ValidationMessages.Required, "Randevu tarihi"))
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Randevu tarihi geçmiş olamaz.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Başlık"))
            .Length(10, 100)
            .WithMessage(string.Format(ValidationMessages.MinMaxLength, "Başlık", "10", "100"));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Açıklama"))
            .Length(10, 500)
            .WithMessage(string.Format(ValidationMessages.MinMaxLength, "Açıklama", "10", "500"));

        RuleFor(x => x.VehicleBrand)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Araç markası"))
            .Length(2, 50)
            .WithMessage(string.Format(ValidationMessages.MinMaxLength, "Araç markası", "2", "50"));

        RuleFor(x => x.VehicleModel)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.Required, "Araç modeli"))
            .Length(2,30)
            .WithMessage(string.Format(ValidationMessages.MinMaxLength, "Araç modeli", "2", "30"));

        RuleFor(x => x.VehicleYear)
          .GreaterThan(1940)
          .WithMessage(string.Format(ValidationMessages.GreaterThan, "Firma Id", "1940"));
    }
}
