using Bookify.Application.Abstractions.Messaging.Commands;

namespace Bookify.Application.Bookings.ReserveBooking;
public record ReserveBookingCommand(
    Guid ApartmentId,
    Guid TenantId,
    DateOnly StartDate,
    DateOnly EndDate) : ICommand<Guid>;