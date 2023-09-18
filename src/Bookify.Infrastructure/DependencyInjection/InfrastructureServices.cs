using Bookify.Infrastructure.Data.SqlConnectionFactory;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.DependencyInjection;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddData();


        return services;
    }
}