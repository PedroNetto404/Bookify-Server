using Bookify.Application.Abstractions.Behaviors;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Services;
using Microsoft.Extensions.DependencyInjection;

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