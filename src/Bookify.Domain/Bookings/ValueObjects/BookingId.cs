namespace Bookify.Domain.Bookings.ValueObjects;

public record BookingId(Guid Value)
{
    public static BookingId New() => new(Guid.NewGuid());
}