using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events; 

public sealed record BookingReservedEvent(Guid BookingId) : IDomainEvent;