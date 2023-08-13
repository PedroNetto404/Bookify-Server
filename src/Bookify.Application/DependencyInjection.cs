using Bookify.Domain.Bookings;
using Microsoft.Extensions.DependencyInjection;
using Namespace;

namespace Bookify.Application; 

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddTransient<PricingService>();

        return services;
    }
}