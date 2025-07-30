using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Bookings.Dtos;
using Adoroid.CarService.Application.Features.Bookings.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Bookings.Qeries.GetByCompanyId;

public record GetByCompanyIdAsyncQuery(PageRequest PageRequest, Guid CompanyId) : IRequest<Response<Paginate<BookingDto>>>;

public class GetByCompanyIdAsyncQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService) : IRequestHandler<GetByCompanyIdAsyncQuery, Response<Paginate<BookingDto>>>
{
    public async Task<Response<Paginate<BookingDto>>> Handle(GetByCompanyIdAsyncQuery request, CancellationToken cancellationToken)
    {
        var redisBookingKey = $"booking:forCompany:{request.CompanyId}";

        var list = await cacheService.GetOrSetListAsync<List<BookingDto>>(redisBookingKey, async () =>
        {
            var bookings = await unitOfWork.Bookings.GetByCompanyIdAsync(request.CompanyId, asNoTracking: true, cancellationToken: cancellationToken);

            var bookingDtos = bookings.Select(b => b.FromEntity()).ToList();

            return bookingDtos;
        }, TimeSpan.FromHours(18));

        return Response<Paginate<BookingDto>>.Success(list.AsQueryable().ToPaginate(request.PageRequest.PageIndex, request.PageRequest.PageSize));
    }
}
