using Bookify.Domain.Apartments.Enums;

namespace Bookify.Domain.Apartments;

public record ApartmentSnapshot
{
    public Guid ApartmentId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string AddressCountry { get; set; } = null!;
    public string AddressState { get; set; } = null!;
    public string AddressCity { get; set; } = null!;
    public string AddressStreet { get; set; } = null!;
    public string AddressNumber { get; set; } = null!;
    public decimal PriceAmount { get; set; }
    public int PriceCurrencyCode { get; set; }
    public decimal CleaningFeeAmount { get; set; }
    public int CleaningFeeCurrencyCode { get; set; }
    public DateTime? LastBookedOnUtc { get; set; }
    public IEnumerable<Amenity> Amenities { get; set; } = null!;
    public string AddressPostalCode { get; set; } = null!;
}