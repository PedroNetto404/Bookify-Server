using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Apartments;
using Bookify.Domain.Apartments.Enums;
using Bookify.Infrastructure.Data.SqlConnectionFactory;
using Bookify.Infrastructure.Data.UnitOfWork;
using Dapper;

namespace Bookify.Infrastructure.Data.Repositories;

internal sealed class ApartmentRepository : BaseRepository, IApartmentRepository, ISearchApartmentQueryProvider
{
    public ApartmentRepository(IDbSession dbSession) : base(dbSession) { }
    
    public async Task<Apartment?> GetByIdAsync(ApartmentId apartmentId)
    {
        const string query = $@"
        SELECT aptAmenity.AmenityCode
        FROM ApartmentAmenity aptAmenity
        WHERE aptAmenity.ApartmentId = @ApartmentId
        
        SELECT 
            apt.ApartmentId AS {nameof(ApartmentSnapshot.ApartmentId)},
            apt.Name AS {nameof(ApartmentSnapshot.Name)},
            apt.Description AS {nameof(ApartmentSnapshot.Description)},
            apt.AddressCountry AS {nameof(ApartmentSnapshot.AddressCountry)},
            apt.AddressState AS {nameof(ApartmentSnapshot.AddressState)},
            apt.AddressCity AS {nameof(ApartmentSnapshot.AddressCity)},
            apt.AddressStreet AS {nameof(ApartmentSnapshot.AddressStreet)},
            apt.AddressNumber AS {nameof(ApartmentSnapshot.AddressNumber)},
            apt.AddressPostalCode AS {nameof(ApartmentSnapshot.AddressPostalCode)},
            apt.PriceAmount AS {nameof(ApartmentSnapshot.PriceAmount)},
            apt.PriceCurrency AS {nameof(ApartmentSnapshot.PriceCurrencyCode)},
            apt.CleaningFeeAmount AS {nameof(ApartmentSnapshot.CleaningFeeAmount)},
            apt.CleaningFeeCurrency AS {nameof(ApartmentSnapshot.CleaningFeeCurrencyCode)},
            apt.LastBookedOnUtc AS {nameof(ApartmentSnapshot.LastBookedOnUtc)}
        FROM 
            Apartment apt
        WHERE 
            apt.ApartmentId = @ApartmentId";

        var gridReader = await Connection.QueryMultipleAsync(
            query,
            new { ApartmentId = apartmentId });

        var apartmentSnapshot = await gridReader.ReadSingleOrDefaultAsync<ApartmentSnapshot>();
        apartmentSnapshot.Amenities = await gridReader.ReadAsync<Amenity>();

        return Apartment.FromSnapshot(apartmentSnapshot);
    }

    public async Task AddAsync(Apartment apartment)
    {
        await InsertApartment();
        await InsertApartmentAmenities();

        async Task<int> InsertApartment()
        {
            const string sql = @$"
            INSERT INTO Apartment
            VALUES
            (
                @{nameof(ApartmentSnapshot.ApartmentId)},
                @{nameof(ApartmentSnapshot.Name)},
                @{nameof(ApartmentSnapshot.Description)},
                @{nameof(ApartmentSnapshot.AddressCountry)},
                @{nameof(ApartmentSnapshot.AddressState)},
                @{nameof(ApartmentSnapshot.AddressCity)},   
                @{nameof(ApartmentSnapshot.AddressStreet)},
                @{nameof(ApartmentSnapshot.AddressPostalCode)},
                @{nameof(ApartmentSnapshot.PriceAmount)},
                @{nameof(ApartmentSnapshot.PriceCurrencyCode)},
                @{nameof(ApartmentSnapshot.CleaningFeeAmount)},
                @{nameof(ApartmentSnapshot.CleaningFeeCurrencyCode)});";

            return await Connection.ExecuteAsync(
                sql, 
                apartment.ToSnapshot(),
                transaction: Transaction);
        }

        async Task<int> InsertApartmentAmenities()
        {
            const string sql = @"
            INSERT INTO ApartmentAmenity 
            VALUES(@ApartmentId, @AmenityCode)";

            return await Connection.ExecuteAsync(
                sql,
                apartment.Amenities.Select(amenity => new
                {
                    apartment.Id,
                    AmenityCode = (int)amenity
                }),
                transaction: Transaction);
        }
    }

    public async Task<IEnumerable<ApartmentResponse>> SearchAsync(DateOnly startDate, DateOnly endDate)
    {
        const string sql = @$"
        SELECT
            apt.ApartmentId AS {nameof(ApartmentResponse.Id)},
            apt.Name AS {nameof(ApartmentResponse.Name)},
            apt.Description AS {nameof(ApartmentResponse.Description)},
            apt.PriceAmount AS {nameof(ApartmentResponse.Price)},
            apt.PriceCurrency AS {nameof(ApartmentResponse.CurrencyCode)},
            apt.AddressCountry AS {nameof(AddressResponse.Country)},
            apt.AddressState AS {nameof(AddressResponse.State)},
            apt.AddressPostalCode AS {nameof(AddressResponse.PostalCode)},
            apt.AddressCity AS {nameof(AddressResponse.City)},
            apt.AddressStreet AS {nameof(AddressResponse.Street)}
        FROM 
            Apartment apt
        WHERE 
            apt.ApartmentId = @ApartmentId";

        return await Connection.QueryAsync<ApartmentResponse>(
            sql,
            new
            {
                StartDate = startDate,
                EndDate = endDate
            }
        );
    }

  
}