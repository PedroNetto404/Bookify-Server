using Bookify.Application.Abstractions.Messaging.Queries;

namespace Bookify.Application.Apartments.SearchApartments;

public record SearchApartmentsQuery(
    DateOnly StartDate,
    DateOnly EndDate
) : IQuery<IReadOnlyList<ApartmentResponse>>;
