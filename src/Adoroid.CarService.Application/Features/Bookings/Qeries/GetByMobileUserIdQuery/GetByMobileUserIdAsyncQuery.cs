using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Bookings.Dtos;
using Adoroid.CarService.Application.Features.Bookings.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Bookings.Qeries.GetByMobileUserIdQuery;

public record GetByMobileUserIdAsyncQuery(PageRequest PageRequest, Guid MobileUserId) : IRequest<Response<Paginate<BookingDto>>>;

public class GetByMobileUserIdAsyncQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService) :
    IRequestHandler<GetByMobileUserIdAsyncQuery, Response<Paginate<BookingDto>>>
{
    public async Task<Response<Paginate<BookingDto>>> Handle(GetByMobileUserIdAsyncQuery request, CancellationToken cancellationToken)
    {
        var redisBookingKey = $"booking:forMobileUser:{request.MobileUserId}";
        var list = await cacheService.GetOrSetListAsync<List<BookingDto>>(redisBookingKey, async () =>
        {
            var bookings = await unitOfWork.Bookings.GetByMobileUserIdAsync(request.MobileUserId, cancellationToken: cancellationToken);
            var bookingDtos = bookings.Select(b => b.FromEntity()).ToList();
            return bookingDtos;

        }, TimeSpan.FromHours(18));

        return Response<Paginate<BookingDto>>.Success(list.AsQueryable().ToPaginate(request.PageRequest.PageIndex, request.PageRequest.PageSize));
    }
}