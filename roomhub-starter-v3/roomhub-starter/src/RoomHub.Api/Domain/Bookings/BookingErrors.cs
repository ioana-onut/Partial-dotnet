using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error BookedByRequired =
        Error.Validation("Booking.BookedByRequired", "BookedBy is required.");

    public static readonly Error InvalidInterval =
        Error.Validation("Booking.InvalidInterval", "EndUtc must be strictly after StartUtc.");

    public static readonly Error InvalidAttendees =
        Error.Validation("Booking.InvalidAttendees", "Attendees must be greater than zero.");

    public static readonly Error AttendeesExceedCapacity =
        Error.Validation("Booking.AttendeesExceedCapacity", "Attendees exceed the room capacity.");

    public static readonly Error StartInPast =
        Error.Validation("Booking.StartInPast", "A booking cannot start in the past.");

    public static readonly Error TooLong =
        Error.Validation("Booking.TooLong", "A booking cannot last more than 4 hours.");

    public static readonly Error RoomOccupied =
        Error.Conflict("Booking.RoomOccupied", "The room already has an active booking in this interval.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("Booking.AlreadyCancelled", "The booking is already cancelled.");

    public static readonly Error NotFound =
        Error.NotFound("Booking.NotFound", "Booking was not found.");
}
