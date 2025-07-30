using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Bookings.Dtos;
using Adoroid.CarService.Application.Features.Bookings.ExceptionMessages;
using Adoroid.CarService.Application.Features.Bookings.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Bookings.Qeries.GetById;

public record BookingGetByIdQuery(Guid BookingId) : IRequest<Response<BookingDto>>;

public class BookingGetByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<BookingGetByIdQuery, Response<BookingDto>>
{
    public async Task<Response<BookingDto>> Handle(BookingGetByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await unitOfWork.Bookings.GetByIdAsync(request.BookingId, cancellationToken: cancellationToken);

        if (booking == null)
            return Response<BookingDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<BookingDto>.Success(booking.FromEntity());
    }
}
