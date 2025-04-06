using Meets.Scheduler.Happenings;
using Meets.Scheduler.Polls;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler;

public static class Registrations
{
    public static IServiceCollection AddGraphQLPresentation(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddInMemorySubscriptions()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<SubscriptionType>()
            .AddType<QueryType>()
            .AddType<MutationType>()
            .AddType<HappeningType>()
            .AddType<UpdateEventInputType>()
            .ModifyOptions(_ => _.DefaultBindingBehavior = BindingBehavior.Explicit);

        services
            .AddHappeningWatch()
            .AddPollWatch();

        return services;
    }
}
