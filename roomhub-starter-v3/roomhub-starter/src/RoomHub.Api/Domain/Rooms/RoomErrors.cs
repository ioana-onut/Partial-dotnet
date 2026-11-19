using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Rooms;

public static class RoomErrors
{
    public static readonly Error NameRequired =
        Error.Validation("Room.NameRequired", "Room name is required.");

    public static readonly Error InvalidCapacity =
        Error.Validation("Room.InvalidCapacity", "Room capacity must be greater than zero.");

    public static readonly Error NotFound =
        Error.NotFound("Room.NotFound", "Room was not found.");
}
