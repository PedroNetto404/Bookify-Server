using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging.Queries;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Apartments.SearchApartments;
internal sealed class SearchApartmentsQueryHandler : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    private static readonly int[] ActiveBookingStatuses = new[]
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    };

    public SearchApartmentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(
        SearchApartmentsQuery request,
        CancellationToken cancellationToken)
    {
        if(request.StartDate > request.EndDate)
        {
            return new List<ApartmentResponse>();
        }

        using var conn = _sqlConnectionFactory.CreateConnection();

        const string sql = """
        SELECT 
            a.id AS Id,
            a.name AS Name,
            a.description AS Description,
            a.price_amount AS PriceAmount,
            a.price_currency AS PriceCurrency,
            a.Address_country AS Country,
            a.Address_state AS State,
            a.Address_zip_code AS ZipCode,
            a.Address_city AS City,
            a.Address_street AS Street
        FROM apartments AS a
        WHERE NOT EXISTS (
            SELECT 1
            FROM bookings AS b
            WHERE b.apartment_id = a.id
            AND b.duration_start <= @EndDate
            AND b.duration_end >= @StartDate
            AND b.status = ANY(@ActiveBookingStatuses)
        )
        """;

        var apartments = await conn.QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
            sql,
             (apartment, address) =>
            {
                apartment.Address = address;
                return apartment;
            },
            new
            {
                request.StartDate,
                request.EndDate,
                ActiveBookingStatuses
            },
            splitOn: "Country");

        return apartments.ToList();
    }
}
