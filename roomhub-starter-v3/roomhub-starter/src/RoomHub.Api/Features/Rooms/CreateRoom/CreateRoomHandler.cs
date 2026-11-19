using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomCommand(string? Name, int Capacity) : ICommand<CreateRoomResponse>;

public sealed record CreateRoomResponse(Guid Id, string Name, int Capacity);

public sealed class CreateRoomHandler(IRoomRepository rooms, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, CreateRoomResponse>
{
    public async Task<Result<CreateRoomResponse>> HandleAsync(
        CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var created = Room.Create(command.Name, command.Capacity);
        if (created.IsFailure)
        {
            return Result<CreateRoomResponse>.Failure(created.Error);
        }

        var room = created.Value;
        rooms.Add(room);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateRoomResponse>.Success(new CreateRoomResponse(room.Id, room.Name, room.Capacity));
    }
}
