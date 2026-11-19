using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingRequest(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

public sealed record CreateBookingCommand(
    Guid RoomId,
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees) : ICommand<CreateBookingResponse>;

public sealed record CreateBookingResponse(Guid Id, Guid RoomId, BookingStatus Status);

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<Result<CreateBookingResponse>> HandleAsync(
        CreateBookingCommand command, CancellationToken cancellationToken)
    {
        var room = await rooms.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
        {
            return Result<CreateBookingResponse>.Failure(RoomErrors.NotFound);
        }

        var activeBookings = await bookings.GetActiveByRoomAsync(room.Id, cancellationToken);
        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

        var created = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            nowUtc,
            activeBookings);

        if (created.IsFailure)
        {
            return Result<CreateBookingResponse>.Failure(created.Error);
        }

        var booking = created.Value;
        bookings.Add(booking);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateBookingResponse>.Success(
            new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status));
    }
}
