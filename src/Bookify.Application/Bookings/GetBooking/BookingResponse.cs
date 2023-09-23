using Bookify.Domain.Bookings;

namespace Bookify.Application.Bookings.GetBooking; 

public record BookingResponse
{
    public Guid Id { get; init; }

    public Guid TenantId { get; init; }

    public Guid ApartmentId { get; init; }

    public int Status { get; init; }

    public decimal PricePerPeriodAmount { get; init; }

    public string PriceCurrency { get; init; }

    public decimal CleaningFeeAmount { get; init; }

    public string CleaningFeeCurrency { get; init; }

    public decimal AmenitiesUpChargeAmount { get; init; }

    public string AmenitiesUpChargeCurrency { get; init; }

    public decimal TotalPriceAmount { get; init; }

    public string TotalPriceCurrency { get; init; }

    public DateOnly DurationStart { get; init; }

    public DateOnly DurationEnd { get; init; }

    public DateTime CreatedOnUtc { get; init; }

    public static implicit operator BookingResponse(Booking booking) =>
        new()
        {
            Id = booking.Id.Value,
            TenantId = booking.TenantId.Value,
            ApartmentId = booking.ApartmentId.Value,
            Status = (int)booking.Status,
            PricePerPeriodAmount = booking.PricePerPeriod.Amount,
            PriceCurrency = booking.PricePerPeriod.Currency.Code,
            CleaningFeeAmount = booking.ClearingFee.Amount,
            CleaningFeeCurrency = booking.ClearingFee.Currency,
            AmenitiesUpChargeAmount = booking.AmenitiesUpCharge.Amount,
            AmenitiesUpChargeCurrency = booking.AmenitiesUpCharge.Currency,
            TotalPriceAmount = booking.TotalPrice.Amount,
            TotalPriceCurrency = booking.TotalPrice.Currency,
            DurationStart = booking.Duration.StartUtc,
            DurationEnd = booking.Duration.EndUtc,
            CreatedOnUtc = booking.CreatedOnUtc
        };
}