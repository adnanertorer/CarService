using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Bookings.Abstracts;

public interface IBookingRepository
{
    Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken);
    Task<Booking?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByCompanyIdAsync(Guid companyId, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByMobileUserIdAsync(Guid mobileUserId, bool asNoTracking = true, CancellationToken cancellationToken = default);
}
