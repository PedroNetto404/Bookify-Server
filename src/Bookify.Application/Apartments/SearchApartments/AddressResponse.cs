namespace Bookify.Application.Apartments.SearchApartments;

public sealed class AddressResponse
{
    public string Country { get; init; } = null!;
    public string State { get; init; } = null!;
    public string PostalCode { get; init; } = null!;
    public string City { get; init; } = null!;
    public string Street { get; init; } = null!;
}