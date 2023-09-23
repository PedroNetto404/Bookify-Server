using Bookify.Domain.Apartments;
using Bookify.Domain.Apartments.Enums;
using Bookify.Domain.Bookings.ValueObjects;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings.Services;
public class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
    {
        var currency = apartment.Price.Currency;

        var pricePorPeriod = new Money(
             apartment.Price.Amount * period.LengthInDays,
             currency);

        decimal percentageUpChange = 0;

        foreach(var amenity in apartment.Amenities)
        {
            percentageUpChange += amenity switch
            {
                Amenity.GardenView or Amenity.MountainView => 0.05m,
                Amenity.AirConditioning => 0.01m,
                Amenity.Parking => 0.01m,
                _ => 0
            };
        }

        var amenitiesUpCharge = Money.Zero(currency);
        if(percentageUpChange > 0)
        {
            amenitiesUpCharge = new Money(
                pricePorPeriod.Amount * percentageUpChange,
                currency);
        }

        var totalPrice = Money.Zero(currency); 
        totalPrice += pricePorPeriod; 

        if(!apartment.CleaningFee.IsZero())
        {
            totalPrice += apartment.CleaningFee; 
        }

        totalPrice += amenitiesUpCharge;

        return new PricingDetails(
            pricePorPeriod, 
            apartment.CleaningFee,
            amenitiesUpCharge,
            totalPrice
        );
    }
}
