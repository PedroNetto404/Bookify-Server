using Bookify.Application.Abstractions.Messaging.Queries;

namespace Bookify.Application.Bookings.GetBooking; 

public record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;