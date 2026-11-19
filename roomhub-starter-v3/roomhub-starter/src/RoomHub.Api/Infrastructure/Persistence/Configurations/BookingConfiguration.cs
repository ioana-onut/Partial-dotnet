using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    private static readonly ValueConverter<DateTime, DateTime> UtcConverter =
        new(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedNever();
        builder.Property(b => b.BookedBy).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Attendees).IsRequired();
        builder.Property(b => b.Status).HasConversion<string>().IsRequired();
        builder.Property(b => b.StartUtc).HasConversion(UtcConverter).IsRequired();
        builder.Property(b => b.EndUtc).HasConversion(UtcConverter).IsRequired();

        builder.HasOne<Room>().WithMany().HasForeignKey(b => b.RoomId);
        builder.HasIndex(b => b.RoomId);
    }
}
