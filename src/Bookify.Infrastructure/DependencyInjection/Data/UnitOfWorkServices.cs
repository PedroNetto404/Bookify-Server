using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Data.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.DependencyInjection.Data;

internal static class UnitOfWorkServices
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbSession, DbSession>();

        return services;
    }
}