using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingCancelledEvent(
    BookingId BookingId) : IDomainEvent;