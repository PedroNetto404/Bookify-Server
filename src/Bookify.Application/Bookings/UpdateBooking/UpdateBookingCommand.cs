using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Domain.Bookings.Enums;

namespace Bookify.Application.Bookings.UpdateBooking
{
    public record UpdateBookingCommand(
        Guid BookingId, 
        BookingStatus newStatus) : ICommand;
}
