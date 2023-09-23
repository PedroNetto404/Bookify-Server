using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Data.SqlConnectionFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.DependencyInjection.Data;

internal static class DbConnectionServices
{
    public static IServiceCollection AddDbConnectionServices(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureOptions<ConnectionString>, ConnectionStringOptionsSetup>();
        services.AddOptions<ConnectionString>();
        services.AddSingleton<ISqlConnectionFactory, SqlServerConnectionFactory>();

        return services;
    }
}