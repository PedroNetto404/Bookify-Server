using Bookify.Domain.Bookings.Enums;

namespace Bookify.Domain.Bookings.ValueObjects;

public record BookingSnapshot
{
    public Guid BookingId { get; set; }
    public Guid ApartmentId { get; set; }
    public Guid TenantId { get; set; }
    public DateOnly DurationStart { get; set; }
    public DateOnly DurationEnd { get; set; }
    public decimal PricePorPeriodAmount { get; set; }
    public int PricePerPeriodCurrencyId { get; set; }
    public decimal ClearingFeeAmount { get; set; }
    public decimal PriceFeeAmount { get; set; }
    public int ClearingFeeCurrencyId { get; set; }
    public int PriceFeeCurrencyCode { get; set; }
    public decimal PriceFeeCurrencyAmount { get; set; }
    public decimal AmenitiesUpChargeAmount { get; set; }
    public int AmenitiesUpChargeCurrencyCode { get; set; }
    public decimal TotalPriceAmount { get; set; }
    public int TotalPriceCurrencyCode { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ConfirmedOnUtc { get; set; }
    public DateTime? RejectedOnUtc { get; set; }
    public DateTime? CompletedOnUtc { get; set; }
    public DateTime? CancelledOnUtc { get; set; }
}