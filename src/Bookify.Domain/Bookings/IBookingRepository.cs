using Bookify.Domain.Apartments;

namespace Bookify.Domain.Bookings;

public interface IBookingRepository
{
    Task AddAsync(Booking booking);
    Task<Booking?> GetByIdAsync(BookingId bookingId);
    Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration);
}