using Meets.Scheduler.Happenings;
using Meets.Scheduler.Votes;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler;

public static class Registrations
{
    public static IServiceCollection AddApplication(this IServiceCollection services) => services
        .AddVote()
        .AddHappening();
}
