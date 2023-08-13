namespace Bookify.Domain.Bookings;

public static class BookingErrors
{
    public static Error NotFound = new(
        "BookingNotFound",
        "Booking with the specified identifier was not found.");

    public static Error Overlap = new(
        "Booking.Overlap",
        "The current booking is overlapping with existing one.");

    public static Error NotReserved = new(
        "Booking.NotReserved",
        "The current booking is not pending.");

    public static Error NotConfirmed = new(
        "Booking.NotConfirmed",
        "The current booking is not confirmed.");

    public static Error AlreadyStarted = new(
        "Booking.AlreadyStarted",
        "The current booking has already started.");

    public static Error NotPending = new(
        "Booking.NotPending",
        "The current booking is not pending.");

    public static Error InvalidDateRange = new(
        "Booking.InvalidDateRange",
        "The current booking has invalid date range.");
}