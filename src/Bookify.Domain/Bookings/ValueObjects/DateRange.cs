using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings;
public record DateRange
{
    private DateRange() {}

    public DateOnly Start { get; init; }

    public DateOnly End { get; init; }

    public int LengthInDays => End.DayNumber - Start.DayNumber;

    public static Result<DateRange> Create(DateOnly start, DateOnly end)
    {
        if (start > end)
        {
            return Result.Failure<DateRange>(BookingErrors.InvalidDateRange);
        }

        return new DateRange
        {
            Start = start,
            End = end
        };
    }
}
