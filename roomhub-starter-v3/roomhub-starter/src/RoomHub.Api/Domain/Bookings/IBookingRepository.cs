namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<Booking>> GetActiveByRoomAsync(Guid roomId, CancellationToken ct = default);

    void Add(Booking booking);
}
