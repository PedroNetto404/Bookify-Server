using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Bookings.ValueObjects;

namespace Bookify.Domain.Bookings;
public sealed class Booking : Entity
{
    private Booking(
        Guid identifier,
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        Money pricePorPeriod,
        Money clearingFee,
        Money amenitiesUpCharge,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc) : base(identifier)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        PricePorPeriod = pricePorPeriod;
        ClearingFee = clearingFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    public Guid ApartmentId { get; private set; }

    public Guid UserId { get; private set; }

    public DateRange Duration { get; private set; }

    public Money PricePorPeriod { get; private set; }

    public Money ClearingFee { get; private set; }

    public Money AmenitiesUpCharge { get; private set; }

    public Money TotalPrice { get; private set; }

    public BookingStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ConfirmedOnUtc { get; private set; }

    public DateTime? RejectedOnUtc { get; private set; }

    public DateTime? CompletedOnUtc { get; private set; }

    public DateTime CancelledOnUtc { get; private set; }

    public static Booking Reserve(
        Apartment apartment,
        Guid userId,
        DateRange duration,
        DateTime utcNow,
        PricingService pricingService)
    {
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);

        var booking = new Booking(
            Guid.NewGuid(),
            apartment.Identifier,
            userId,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved,
            utcNow
        );

        booking.RaiseDomainEvents(new BookingReservedEvent(booking.Identifier));

        apartment.LastBookedOnUtc = utcNow;        

        return booking;
    }

    public Result Confirm(DateTime utcNow)
    {
        if(Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotPending);
        }        

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcNow;

        RaiseDomainEvents(new BookingConfirmedEvent(Identifier));

        return Result.Success();
    }

    public Result Reject(DateTime utcNow)
    {
        if(Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotPending);
        }

        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcNow;

        RaiseDomainEvents(new BookingRejectedEvent(Identifier));

        return Result.Success();
    }

    public Result Complete(DateTime utcNow)
    {
        if(Status != BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }

        Status = BookingStatus.Completed;
        CompletedOnUtc = utcNow;

        RaiseDomainEvents(new BookingCompletedEvent(Identifier));

        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        if(Status != BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }

        var currentDate = DateOnly.FromDateTime(utcNow);
        if(currentDate > Duration.Start)
        {
            return Result.Failure(BookingErrors.AlreadyStarted);
        }

        Status = BookingStatus.Cancelled;
        CancelledOnUtc = utcNow;

        RaiseDomainEvents(new BookingCancelledEvent(Identifier));

        return Result.Success();
    }
}
