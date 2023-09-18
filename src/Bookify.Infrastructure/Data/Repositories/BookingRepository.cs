using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Infrastructure.Data.SqlConnectionFactory;
using Dapper;

namespace Bookify.Infrastructure.Data.Repositories;

internal sealed class BookingRepository : BaseRepository, IBookingRepository
{
    public BookingRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
    {
    }

    private static readonly int[] ActiveBookingStatuses = new[]
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    };

    public async Task AddAsync(Booking booking)
    {
        using var conn = GetOpeningConnection();

        const string sql = $@"
        INSERT INTO Booking(
           BookingId,
           ApartmentId,
           TenantId,
           DurationStartUtc,
           DurationEndUtc,
           PriceAmount,
           PriceCurrencyId,
           CleaningFeeAmount,
           CleaningFeeCurrencyId,
           AmenitiesUpChargeAmount,
           AmenitiesUpChargeCurrencyId,
           TotalPriceAmount,
           TotalPriceCurrencyId,
           Status,
           CreatedOnUtc
        VALUES (
           @{nameof(BookingSnapshot.BookingId)},
           @{nameof(BookingSnapshot.ApartmentId)},
           @{nameof(BookingSnapshot.TenantId)},
           @{nameof(BookingSnapshot.DurationStart)},
           @{nameof(BookingSnapshot.DurationEnd)},
           @{nameof(BookingSnapshot.PricePorPeriodAmount)},
           @{nameof(BookingSnapshot.PricePerPeriodCurrencyId)},
           @{nameof(BookingSnapshot.ClearingFeeAmount)},
           @{nameof(BookingSnapshot.ClearingFeeCurrencyId)},
           @{nameof(BookingSnapshot.AmenitiesUpChargeAmount)},
           @{nameof(BookingSnapshot.AmenitiesUpChargeCurrencyCode)},
           @{nameof(BookingSnapshot.TotalPriceAmount)},
           @{nameof(BookingSnapshot.TotalPriceCurrencyCode)},
           @{nameof(BookingSnapshot.Status)},
           @{nameof(BookingSnapshot.CreatedOnUtc)});";

        await conn.ExecuteAsync(sql, booking.ToSnapshot());
    }

    public async Task<Booking?> GetByIdAsync(BookingId bookingId)
    {
        using var connection = GetOpeningConnection();

        const string sql = @$"
        SELECT
            BookingId AS {nameof(BookingSnapshot.BookingId)},
            ApartmentId AS {nameof(BookingSnapshot.ApartmentId)},
            TenantId AS {nameof(BookingSnapshot.TenantId)},
            DurationStartUtc AS {nameof(BookingSnapshot.DurationStart)},
            DurationEndUtc AS {nameof(BookingSnapshot.DurationEnd)},
            PriceAmount AS {nameof(BookingSnapshot.PricePorPeriodAmount)},
            PriceCurrencyId AS {nameof(BookingSnapshot.PricePerPeriodCurrencyId)},
            CleaningFeeAmount AS {nameof(BookingSnapshot.ClearingFeeAmount)},
            CleaningFeeCurrencyId AS {nameof(BookingSnapshot.ClearingFeeCurrencyId)},
            AmenitiesUpChargeAmount AS {nameof(BookingSnapshot.AmenitiesUpChargeAmount)},
            AmenitiesUpChargeCurrencyId AS {nameof(BookingSnapshot.AmenitiesUpChargeCurrencyCode)},
            TotalPriceAmount AS {nameof(BookingSnapshot.TotalPriceAmount)},
            TotalPriceCurrencyId AS {nameof(BookingSnapshot.TotalPriceCurrencyCode)},
            Status AS {nameof(BookingSnapshot.Status)},
            CreatedOnUtc AS {nameof(BookingSnapshot.CreatedOnUtc)},
            ConfirmedOnUtc AS {nameof(BookingSnapshot.ConfirmedOnUtc)},
            RejectedOnUtc AS {nameof(BookingSnapshot.RejectedOnUtc)},
            CompletedOnUtc AS {nameof(BookingSnapshot.CompletedOnUtc)},
            CancelledOnUtc AS {nameof(BookingSnapshot.CancelledOnUtc)}
        FROM Booking
        WHERE BookingId = @BookingId";

        var bookingSnapshot = await connection.QueryFirstOrDefaultAsync<BookingSnapshot>(
            sql,
            new { BookingId = bookingId.Value });

        return bookingSnapshot is null ? null : Booking.FromSnapshot(bookingSnapshot);
    }

    public async Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration)
    {
        var connection = GetOpeningConnection();

        const string sql = @"
        SELECT EXISTS (
            SELECT 
                1
            FROM 
                Booking
            WHERE 
                ApartmentId = @ApartmentId AND
                DurationStartUtc <= @DurationEndUtc AND
                DurationEndUtc >= @DurationStartUtc AND
                Status IN (@ActiveBookingStatuses)
        )";

        return await connection.ExecuteScalarAsync<bool>(
            sql,
            new
            {
                ApartmentId = apartment.Id.Value,
                DurationStartUtc = duration.StartUtc,
                DurationEndUtc = duration.EndUtc,
                ActiveBookingStatuses
            });
    }
}