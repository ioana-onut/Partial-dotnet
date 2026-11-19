using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Common.Http;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", async (
            Guid id,
            ICommandHandler<CancelBookingCommand, Unit> handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new CancelBookingCommand(id), ct);

            return result.IsFailure
                ? result.Error.ToProblem()
                : Results.NoContent();
        });
    }
}
