using Meets.Scheduler.Happenings;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler;

public static class Registrations
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services
            .AddScoped<IHappeningCreationManager, HappeningCreationManager>();
    }
}
