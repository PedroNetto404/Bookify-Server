using Bookify.Domain.Utility.Results;

namespace Bookify.Domain.Bookings.ValueObjects;
public record DateRange
{
    private DateRange(DateOnly start, DateOnly end) =>
        (StartUtc, EndUtc) = (start, end);

    public DateOnly StartUtc { get; init; }

    public DateOnly EndUtc { get; init; }

    public int LengthInDays => EndUtc.DayNumber - StartUtc.DayNumber;

    public static Result<DateRange> Create(DateOnly start, DateOnly end) =>
        start < end
            ? new DateRange(start, end)
            : Result.Failure<DateRange>(BookingErrors.InvalidDateRange);
}
