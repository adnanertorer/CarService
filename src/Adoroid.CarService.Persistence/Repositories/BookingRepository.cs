using Adoroid.CarService.Application.Features.Bookings.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class BookingRepository(CarServiceDbContext dbContext) : IBookingRepository
{
    public async Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken)
    {
        await dbContext.Bookings.AddAsync(booking, cancellationToken);
        return booking;
    }

    public async Task<IEnumerable<Booking>> GetByCompanyIdAsync(Guid companyId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking 
            ? dbContext.Bookings
            .Include(i => i.MobileUser)
            .AsNoTracking() 
            : dbContext.Bookings
            .Include(i => i.MobileUser)
            .AsQueryable();

        return await query.Where(b => b.CompanyId == companyId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Booking?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        return await (asNoTracking 
            ? dbContext.Bookings.AsNoTracking() 
            : dbContext.Bookings.AsQueryable())
            .Include(i => i.MobileUser)
            .Include(i => i.Company)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetByMobileUserIdAsync(Guid mobileUserId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking
           ? dbContext.Bookings
           .Include(i => i.Company)
           .AsNoTracking()
           : dbContext.Bookings
           .Include(i => i.Company)
           .AsQueryable();

        return await query.Where(b => b.MobileUserId == mobileUserId)
            .ToListAsync(cancellationToken);
    }
}
