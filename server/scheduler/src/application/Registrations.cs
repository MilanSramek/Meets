using Meets.Scheduler.Activities;
using Meets.Scheduler.Votes;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler;

public static class Registrations
{
    public static IServiceCollection AddApplication(this IServiceCollection services) => services
        .AddVote()
        .AddActivity();
}
