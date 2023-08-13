using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments;

public sealed class Apartment : Entity
{
    public Apartment(
        Guid identifier,
        Name name, 
        Description description, 
        Address address, 
        Money priceAmount, 
        Money cleaningFeeAmount, 
        List<Amenity> amenities) : base(identifier)
    {
        Name = name;
        Description = description;
        Address = address;
        Price = priceAmount;
        CleaningFee = cleaningFeeAmount;
    }

    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public Money CleaningFee { get; private set; }
    public DateTime? LastBookedOnUtc { get; internal set; }
    public List<Amenity> Amenities { get; private set; } = new();
}