using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings.ValueObjects;

namespace Bookify.Domain.Bookings.Events; 

public sealed record BookingReservedEvent(
    BookingId BookingId) : IDomainEvent;