using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Bookings.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Bookings.Commands.Delete;

public record DeleteBookingCommand(Guid BookingId) : IRequest<Response<Guid>>;

public class DeleteBookingCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<DeleteBookingCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await unitOfWork.Bookings.GetByIdAsync(request.BookingId, cancellationToken: cancellationToken);
        if (booking == null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        var userId = currentUser.ValidUserId();

        if(booking.MobileUserId != userId)
            return Response<Guid>.Fail(Common.BusinessMessages.BusinessMessages.UnauthorizedAction);

        booking.IsDeleted = true;
        booking.DeletedDate = DateTime.UtcNow;
        booking.DeletedBy = Guid.Parse(currentUser.Id!);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Response<Guid>.Success(booking.Id);
    }
}
