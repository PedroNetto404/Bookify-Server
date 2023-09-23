using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.Abstractions;
using Bookify.Domain.Tenants;
using Bookify.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.DependencyInjection.Data;

internal static class RepositoriesServices
{
    public static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IApartmentRepository, ApartmentRepository>()
                .AddScoped<ISearchApartmentQueryProvider, ApartmentRepository>()
                .AddScoped<IBookingRepository, BookingRepository>()
                .AddScoped<ITenantRepository, TenantRepository>();
}