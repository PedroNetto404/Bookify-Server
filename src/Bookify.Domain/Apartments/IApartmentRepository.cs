namespace Bookify.Domain.Apartments; 

public interface IApartmentRepository
{
    Task<Apartment?> GetByIdAsync(ApartmentId apartmentId);

    Task AddAsync(Apartment apartment);
}