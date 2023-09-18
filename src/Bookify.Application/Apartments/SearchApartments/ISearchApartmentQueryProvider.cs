namespace Bookify.Application.Apartments.SearchApartments;

public interface ISearchApartmentQueryProvider
{
    Task<IEnumerable<ApartmentResponse>> SearchAsync(DateOnly startDate, DateOnly endDate);
}