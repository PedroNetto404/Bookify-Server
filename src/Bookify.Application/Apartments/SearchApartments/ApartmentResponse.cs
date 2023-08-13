namespace Bookify.Application.Apartments.SearchApartments;

public sealed class ApartmentResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public string Currency { get; init; }

    public AddressResponse Address { get; set; }
}


public sealed class AddressResponse
{
    public string Country { get; init; }
    
    public string State { get; init; }
    
    public string ZipCode { get; init; }

    public string city { get; init; }
    
    public string Street { get; init; }
}