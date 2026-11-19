using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public static class GetRoomEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rooms/{id:guid}", async (
            Guid id,
            IQueryHandler<GetRoomQuery, GetRoomResponse> handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new GetRoomQuery(id), ct);

            return result.IsFailure
                ? result.Error.ToProblem()
                : Results.Ok(result.Value);
        });
    }
}
