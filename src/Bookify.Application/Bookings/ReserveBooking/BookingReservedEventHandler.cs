using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Abstractions;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Tenants;
using MediatR;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class BookingReservedEventHandler : INotificationHandler<BookingReservedEvent>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IEmailService _emailService;
    public BookingReservedEventHandler(ITenantRepository tenantRepository, IBookingRepository bookingRepository)
    {
        _tenantRepository = tenantRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(BookingReservedEvent notification, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(notification.BookingId);
        if(booking is null) return;

        var user = await _tenantRepository.GetByIdAsync(booking.TenantId);
        if(user is null) return;

        await _emailService.SendAsync(
            user.Email,
            "Booking reserved!",
            $"Your have 10 minutes to confirm this booking");

    }
}