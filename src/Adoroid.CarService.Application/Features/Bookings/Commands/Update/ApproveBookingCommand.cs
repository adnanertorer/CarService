using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Bookings.Dtos;
using Adoroid.CarService.Application.Features.Bookings.ExceptionMessages;
using Adoroid.CarService.Application.Features.Bookings.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Bookings.Commands.Update;

public record ApproveBookingCommand(Guid BookingId) : IRequest<Response<BookingDto>>;

public class ApproveBookingCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<ApproveBookingCommand, Response<BookingDto>>
{
    public async Task<Response<BookingDto>> Handle(ApproveBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await unitOfWork.Bookings.GetByIdAsync(request.BookingId, cancellationToken: cancellationToken);

        if (booking == null)
            return Response<BookingDto>.Fail(BusinessExceptionMessages.NotFound);

        var companyId = currentUser.ValidCompanyId();

        if (currentUser.UserType == "company" && companyId == booking.CompanyId)
        {
            booking.IsApproved = true;
            booking.UpdatedDate = DateTime.UtcNow;
            booking.UpdatedBy = Guid.Parse(currentUser.Id!);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Response<BookingDto>.Success(booking.FromEntity());

            //kullanıcı mail ya da sms yoluyla bilgilendirilecek
        }

        return Response<BookingDto>.Fail(Common.BusinessMessages.BusinessMessages.UnauthorizedAction);
    }
}
