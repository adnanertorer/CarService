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
        IQueryable<Booking> query;

        if(asNoTracking)
        {
            query = dbContext.Bookings
                .Include(i => i.MobileUser)
                .AsNoTracking()
                .AsQueryable();
        }
        else
        {
            query = dbContext.Bookings
                .Include(i => i.MobileUser)
                .AsQueryable();
        }

        return await query.Where(b => b.CompanyId == companyId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Booking?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (asNoTracking)
        {
            return await dbContext.Bookings
                .AsNoTracking()
                .Include(i => i.MobileUser)
                .Include(i => i.Company)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        return await dbContext.Bookings
            .Include(i => i.MobileUser)
            .Include(i => i.Company)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetByMobileUserIdAsync(Guid mobileUserId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<Booking> query;

        if (asNoTracking)
        {
            query = dbContext.Bookings
                .Include(i => i.Company)
                .AsNoTracking()
                .AsQueryable();
        }
        else
        {
            query = dbContext.Bookings
                .Include(i => i.Company)
                .AsQueryable();
        }

        return await query.Where(b => b.MobileUserId == mobileUserId)
            .ToListAsync(cancellationToken);
    }
}
