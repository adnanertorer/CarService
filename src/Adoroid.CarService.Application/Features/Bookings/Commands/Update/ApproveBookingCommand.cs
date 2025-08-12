using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Bookings.Dtos;
using Adoroid.CarService.Application.Features.Bookings.ExceptionMessages;
using Adoroid.CarService.Application.Features.Bookings.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Repositories;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Bookings.Commands.Update;

public record ApproveBookingCommand(Guid BookingId, string? CompanyMessage) : IRequest<Response<BookingDto>>;

public class ApproveBookingCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICacheService cacheService, ILogger<ApproveBookingCommandHandler> logger) : IRequestHandler<ApproveBookingCommand, Response<BookingDto>>
{
    public async Task<Response<BookingDto>> Handle(ApproveBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await unitOfWork.Bookings.GetByIdAsync(request.BookingId, false, cancellationToken: cancellationToken);

        if (booking == null)
            return Response<BookingDto>.Fail(BusinessExceptionMessages.NotFound);

        var redisKeyPrefix = $"booking:forCompany:{booking.CompanyId}";

        var companyId = currentUser.ValidCompanyId();

        if (currentUser.UserType == "company" && companyId == booking.CompanyId)
        {
            booking.Status = (int)BookingStatusEnum.Approved;
            booking.CompanyMessage = request.CompanyMessage;
            booking.UpdatedDate = DateTime.UtcNow;
            booking.UpdatedBy = Guid.Parse(currentUser.Id!);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = booking.FromEntity();

            logger.LogInformation("Booking with ID {BookingId} approved by company {CompanyId}.", request.BookingId, companyId);

            try
            {
                await cacheService.UpdateToListAsync(redisKeyPrefix, request.BookingId.ToString(), resultDto, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while updating to cache for sub service update.");
            }

            return Response<BookingDto>.Success(resultDto);

            //kullanıcı mail ya da sms yoluyla bilgilendirilecek
        }

        return Response<BookingDto>.Fail(Common.BusinessMessages.BusinessMessages.UnauthorizedAction);
    }
}
