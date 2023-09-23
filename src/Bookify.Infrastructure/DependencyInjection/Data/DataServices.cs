using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.DependencyInjection.Data;

internal static class DataServices
{
    public static IServiceCollection AddData(this IServiceCollection services) =>
        services
            .AddRepositories()
            .AddUnitOfWork()
            .AddDbConnectionServices();
}
