using Bookify.Application.Abstractions.Messaging.Queries;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Abstractions;
using Bookify.Domain.Bookings.ValueObjects;
using Bookify.Domain.Errors;
using Bookify.Domain.Utility.Results;
using Dapper;

namespace Bookify.Application.Bookings.GetBooking;

internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(new BookingId(request.BookingId));
        if (booking is null) return DomainErrors.Booking.NotFound;

        return Result.Success<BookingResponse>(booking);
    }
}