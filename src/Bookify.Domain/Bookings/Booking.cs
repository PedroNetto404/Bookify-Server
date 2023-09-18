using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Users;

namespace Bookify.Domain.Bookings;

public sealed class Booking : AggregateRoot<BookingId>
{
    private Booking(
        ApartmentId apartmentId,
        TenantId tenantId,
        DateRange duration,
        Money pricePerPeriod,
        Money clearingFee,
        Money amenitiesUpCharge,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc) : base(BookingId.New())
    {
        ApartmentId = apartmentId;
        TenantId = tenantId;
        Duration = duration;
        PricePerPeriod = pricePerPeriod;
        ClearingFee = clearingFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    public static readonly BookingStatus[] ActiveBookingStatuses =
    {
        BookingStatus.Reserved,
        BookingStatus.Confirmed,
        BookingStatus.Completed
    };

    public ApartmentId ApartmentId { get; private set; }

    public TenantId TenantId { get; private set; }

    public DateRange Duration { get; private set; }

    public Money PricePerPeriod { get; private set; }

    public Money ClearingFee { get; private set; }

    public Money AmenitiesUpCharge { get; private set; }

    public Money TotalPrice { get; private set; }

    public BookingStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ConfirmedOnUtc { get; private set; }

    public DateTime? RejectedOnUtc { get; private set; }

    public DateTime? CompletedOnUtc { get; private set; }

    public DateTime? CancelledOnUtc { get; private set; }

    public static Booking Reserve(
        Apartment apartment,
        TenantId tenantId,
        DateRange duration,
        DateTime utcNow,
        PricingService pricingService)
    {
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);

        var booking = new Booking(
            apartment.Id,
            tenantId,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved,
            utcNow
        );

        booking.RaiseDomainEvents(new BookingReservedEvent(booking.Id));

        apartment.LastBookedOnUtc = utcNow;

        return booking;
    }

    public Result Confirm(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotPending);
        }

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcNow;

        RaiseDomainEvents(new BookingConfirmedEvent(Id));

        return Result.Success();
    }

    public Result Reject(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotPending);
        }

        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcNow;

        RaiseDomainEvents(new BookingRejectedEvent(Id));

        return Result.Success();
    }

    public Result Complete(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }

        Status = BookingStatus.Completed;
        CompletedOnUtc = utcNow;

        RaiseDomainEvents(new BookingCompletedEvent(Id));

        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }

        var currentDate = DateOnly.FromDateTime(utcNow);
        if (currentDate > Duration.StartUtc)
        {
            return Result.Failure(BookingErrors.AlreadyStarted);
        }

        Status = BookingStatus.Cancelled;
        CancelledOnUtc = utcNow;

        RaiseDomainEvents(new BookingCancelledEvent(Id));

        return Result.Success();
    }

    #region Snapshot
    public static Booking FromSnapshot(BookingSnapshot snapshot) =>
        new(
            new ApartmentId(snapshot.ApartmentId),
            new TenantId(snapshot.TenantId),
            DateRange.Create(snapshot.DurationStart, snapshot.DurationEnd).Value,
            new Money(snapshot.PricePorPeriodAmount, snapshot.PricePerPeriodCurrencyId),
            new Money(snapshot.ClearingFeeAmount, snapshot.ClearingFeeCurrencyId),
            new Money(snapshot.AmenitiesUpChargeAmount, snapshot.AmenitiesUpChargeCurrencyCode),
            new Money(snapshot.TotalPriceAmount, snapshot.TotalPriceCurrencyCode),
            snapshot.Status,
            snapshot.CreatedOnUtc
        )
        {
            Id = new BookingId(snapshot.BookingId),
            ConfirmedOnUtc = snapshot.ConfirmedOnUtc,
            RejectedOnUtc = snapshot.RejectedOnUtc,
            CompletedOnUtc = snapshot.CompletedOnUtc,
            CancelledOnUtc = snapshot.CancelledOnUtc
        };

    public BookingSnapshot ToSnapshot() =>
        new()
        {
            BookingId = Id.Value,
            ApartmentId = ApartmentId.Value,
            TenantId = TenantId.Value,
            DurationStart = Duration.StartUtc,
            DurationEnd = Duration.EndUtc,
            PricePorPeriodAmount = PricePerPeriod.Amount,
            PricePerPeriodCurrencyId = PricePerPeriod.Currency.Id,
            ClearingFeeAmount = ClearingFee.Amount,
            ClearingFeeCurrencyId = ClearingFee.Currency.Id,
            AmenitiesUpChargeAmount = AmenitiesUpCharge.Amount,
            AmenitiesUpChargeCurrencyCode = AmenitiesUpCharge.Currency.Id,
            TotalPriceAmount = TotalPrice.Amount,
            TotalPriceCurrencyCode = TotalPrice.Currency.Id,
            Status = Status,
            CreatedOnUtc = CreatedOnUtc,
            ConfirmedOnUtc = ConfirmedOnUtc,
            RejectedOnUtc = RejectedOnUtc,
            CompletedOnUtc = CompletedOnUtc,
            CancelledOnUtc = CancelledOnUtc
        };

    #endregion
}