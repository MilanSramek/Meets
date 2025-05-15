using Meets.Scheduler.Activities;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler;

public static class Registrations
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services
            .AddScoped<IActivityCreationManager, ActivityCreationManager>();
    }
}
