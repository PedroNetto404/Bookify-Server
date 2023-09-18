using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Data.Repositories;
using Bookify.Infrastructure.Data.SqlConnectionFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.DependencyInjection;

internal static class DataServices
{
    public static IServiceCollection AddData(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlServerConnectionFactory>();

        ConfigureOptions(services);

        services.AddScoped<IApartmentRepository, ApartmentRepository>();
        services.AddScoped<ISearchApartmentQueryProvider, ApartmentRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();

        return services;
    }

    private static void ConfigureOptions(IServiceCollection services)
    {
        services.AddOptions<ConnectionString>();
        services.AddSingleton<IConfigureOptions<ConnectionString>, ConnectionStringOptionsSetup>();
    }
}
