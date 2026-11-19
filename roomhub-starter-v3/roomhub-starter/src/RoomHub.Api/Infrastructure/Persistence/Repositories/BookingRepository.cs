using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<IReadOnlyList<Booking>> GetActiveByRoomAsync(Guid roomId, CancellationToken ct = default) =>
        await db.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(ct);

    public void Add(Booking booking) => db.Bookings.Add(booking);
}
