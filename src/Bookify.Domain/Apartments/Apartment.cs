using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments.Enums;
using Bookify.Domain.Apartments.ValueObjects;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Apartments;

public sealed class Apartment : AggregateRoot<ApartmentId>
{
    public Apartment(
        Name name,
        Description description,
        Address address,
        Money priceAmount,
        Money cleaningFeeAmount,
        params Amenity[] amenities) : base(ApartmentId.New())
    {
        Name = name;
        Description = description;
        Address = address;
        Price = priceAmount;
        CleaningFee = cleaningFeeAmount;

        _amenities.AddRange(amenities);
    }

    private readonly List<Amenity> _amenities = new();

    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public Money CleaningFee { get; private set; }
    public DateTime? LastBookedOnUtc { get; internal set; }
    public IReadOnlyCollection<Amenity> Amenities => _amenities.AsReadOnly();

    #region Snapshot

    public static Apartment FromSnapshot(ApartmentSnapshot snapshot) =>
        new(
            new Name(snapshot.Name),
            new Description(snapshot.Description),
            new Address(
                snapshot.AddressCountry,
                snapshot.AddressState,
                snapshot.AddressCity,
                snapshot.AddressStreet,
                snapshot.AddressPostalCode),
            new Money(snapshot.PriceAmount, snapshot.PriceCurrencyCode),
            new Money(snapshot.CleaningFeeAmount, snapshot.CleaningFeeCurrencyCode),
            snapshot.Amenities.ToArray())
        {
            LastBookedOnUtc = snapshot.LastBookedOnUtc
        };

    public ApartmentSnapshot ToSnapshot() =>
        new()
        {
            ApartmentId = Id.Value,
            Name = Name.Value,
            Description = Description.Value,
            AddressCountry = Address.Country,
            AddressState = Address.State,
            AddressCity = Address.City,
            AddressStreet = Address.Street,
            PriceAmount = Price.Amount,
            PriceCurrencyCode = Price.Currency.Id,
            CleaningFeeAmount = CleaningFee.Amount,
            CleaningFeeCurrencyCode = CleaningFee.Currency.Id,
        };

    #endregion
}