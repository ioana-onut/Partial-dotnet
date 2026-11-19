using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;
using RoomHub.Api.Infrastructure.Persistence;
using RoomHub.Api.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RoomHubDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RoomHub") ?? "Data Source=roomhub.db"));

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RoomHubDbContext>());
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddHandlers(typeof(Program).Assembly);
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<RoomHubDbContext>().Database.EnsureCreated();
}

app.MapGet("/", () => "RoomHub API is running.");
CreateRoomEndpoint.Map(app);
GetRoomEndpoint.Map(app);
CreateBookingEndpoint.Map(app);
GetBookingEndpoint.Map(app);
CancelBookingEndpoint.Map(app);

app.Run();
