using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.ValueObjects;

namespace Bookify.Domain.Bookings.Abstractions;

public interface IBookingRepository
{
    Task AddAsync(Booking booking);
    Task<Booking?> GetByIdAsync(BookingId bookingId);
    Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration);
    Task UpdateAsync(Booking booking);
}