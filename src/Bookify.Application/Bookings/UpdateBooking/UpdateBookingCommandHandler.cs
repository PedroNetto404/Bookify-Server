using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Abstractions;
using Bookify.Domain.Bookings.Enums;
using Bookify.Domain.Bookings.ValueObjects;
using Bookify.Domain.Errors;
using Bookify.Domain.Utility.Results;

namespace Bookify.Application.Bookings.UpdateBooking
{
    public class UpdateBookingCommandHandler : ICommandHandler<UpdateBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateBookingCommandHandler(
            IBookingRepository bookingRepository,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(new BookingId(request.BookingId));
            if (booking is null) return DomainErrors.Booking.NotFound;

            _updateActions[request.newStatus](booking, _dateTimeProvider.UtcNow);

            await _bookingRepository.UpdateAsync(booking);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }

        private readonly Dictionary<BookingStatus, Action<Booking, DateTime>> _updateActions =
            new()
            {
                {
                    BookingStatus.Confirmed, (booking, now) => booking.Confirm(now)
                },
                {
                    BookingStatus.Completed, (booking, now) => booking.Complete(now)
                },
                {
                    BookingStatus.Cancelled, (booking, now) => booking.Cancel(now)
                },
                {
                    BookingStatus.Rejected, (booking, now) => booking.Reject(now)
                }
            };
    }
}
