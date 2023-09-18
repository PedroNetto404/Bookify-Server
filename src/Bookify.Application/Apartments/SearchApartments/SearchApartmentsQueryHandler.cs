using Bookify.Application.Abstractions.Messaging.Queries;
using Bookify.Domain.Abstractions;

namespace Bookify.Application.Apartments.SearchApartments;

internal sealed class
    SearchApartmentsQueryHandler : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
{
    private readonly ISearchApartmentQueryProvider _searchApartmentQueryProvider;

    public SearchApartmentsQueryHandler(
        ISearchApartmentQueryProvider searchApartmentQueryProvider)
    {
        _searchApartmentQueryProvider = searchApartmentQueryProvider;
    }

    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(
        SearchApartmentsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
        {
            return new List<ApartmentResponse>();
        }

        var query = await _searchApartmentQueryProvider.SearchAsync(
            request.StartDate, 
            request.EndDate);

        return query.ToList();
    }
}