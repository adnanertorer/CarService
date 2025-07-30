using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Bookings.Dtos;
using Adoroid.CarService.Application.Features.Bookings.ExceptionMessages;
using Adoroid.CarService.Application.Features.Bookings.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Bookings.Commands.Create;

public record CreateBookingCommand(Guid CompanyId, DateTime BookingDate, string Title, string Description, string VehicleBrand,
    string VehicleModel, int VehicleYear): IRequest<Response<BookingDto>>;

public class CreateBookingCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<CreateBookingCommand, Response<BookingDto>>
{
    public async Task<Response<BookingDto>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var isExistCompany = await unitOfWork.Companies.IsCompanyExistsAsync(request.CompanyId, cancellationToken: cancellationToken);

        if (!isExistCompany)
            return Response<BookingDto>.Fail(BusinessExceptionMessages.CompanyNotFound);

        var booking = new Booking
        {
            BookingDate = request.BookingDate,
            CompanyId = request.CompanyId,
            CreatedBy = Guid.Parse(currentUser.Id!),
            CreatedDate = DateTime.UtcNow,
            Description = request.Description,
            IsApproved = false,
            IsDeleted = false,
            MobileUserId = Guid.Parse(currentUser.Id!),
            Title = request.Title,
            VehicleBrand = request.VehicleBrand,
            VehicleModel = request.VehicleModel,
            VehicleYear = request.VehicleYear
        };

        await unitOfWork.Bookings.AddAsync(booking, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<BookingDto>.Success(booking.FromEntity());
    }
}
