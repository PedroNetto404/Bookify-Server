using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingCancelledEvent(Guid BookingId) : IDomainEvent;