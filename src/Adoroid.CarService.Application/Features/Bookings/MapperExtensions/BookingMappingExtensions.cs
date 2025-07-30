using Adoroid.CarService.Application.Features.Bookings.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Bookings.MapperExtensions;

public static class BookingMappingExtensions
{
    public static BookingDto FromEntity(this Booking booking)
    {
        return new BookingDto
        {
            BookingDate = booking.BookingDate,
            CompanyId = booking.CompanyId,
            Description = booking.Description,
            IsApproved = booking.IsApproved,
            Id = booking.Id,
            MobileUserId = booking.MobileUserId,
            Title = booking.Title,
            VehicleBrand = booking.VehicleBrand,
            VehicleModel = booking.VehicleModel,
            VehicleYear = booking.VehicleYear,
            Company = booking.Company != null ? new CompanyDto
            {
                Id = booking.Company.Id,
                Name = booking.Company.CompanyName,
                Address = booking.Company.CompanyAddress,
                PhoneNumber = booking.Company.CompanyPhone
            } : null,
            MobileUser = booking.MobileUser != null ? new MobileUserDto
            {
                Id = booking.MobileUser.Id,
                Name = booking.MobileUser.Name,
                Surname = booking.MobileUser.Surname,
                Email = booking.MobileUser.Email
            } : null
        };
    }
}
