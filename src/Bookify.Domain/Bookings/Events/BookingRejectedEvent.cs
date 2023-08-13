using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingRejectedEvent(Guid BookingId) : IDomainEvent;