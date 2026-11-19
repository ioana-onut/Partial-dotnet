using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings", async (
            Guid roomId,
            CreateBookingRequest request,
            ICommandHandler<CreateBookingCommand, CreateBookingResponse> handler,
            CancellationToken ct) =>
        {
            var command = new CreateBookingCommand(
                roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees);

            var result = await handler.HandleAsync(command, ct);

            return result.IsFailure
                ? result.Error.ToProblem()
                : Results.Created($"/api/bookings/{result.Value.Id}", result.Value);
        });
    }
}
