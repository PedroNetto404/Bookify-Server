using Bookify.Infrastructure.DependencyInjection.Data;
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