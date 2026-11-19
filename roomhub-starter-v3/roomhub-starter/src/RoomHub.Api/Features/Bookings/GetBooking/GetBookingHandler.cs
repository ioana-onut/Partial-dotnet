using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid Id) : IQuery<GetBookingResponse>;

public sealed record GetBookingResponse(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    BookingStatus Status);

public sealed class GetBookingHandler(IBookingRepository bookings)
    : IQueryHandler<GetBookingQuery, GetBookingResponse>
{
    public async Task<Result<GetBookingResponse>> HandleAsync(
        GetBookingQuery query, CancellationToken cancellationToken)
    {
        var booking = await bookings.GetByIdAsync(query.Id, cancellationToken);
        if (booking is null)
        {
            return Result<GetBookingResponse>.Failure(BookingErrors.NotFound);
        }

        return Result<GetBookingResponse>.Success(new GetBookingResponse(
            booking.Id,
            booking.RoomId,
            booking.BookedBy,
            booking.StartUtc,
            booking.EndUtc,
            booking.Attendees,
            booking.Status));
    }
}
