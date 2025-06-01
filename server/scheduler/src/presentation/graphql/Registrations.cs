using Meets.Common.Presentation.GraphQL;
using Meets.Scheduler.Activities;
using Meets.Scheduler.Polls;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler;

public static class Registrations
{
    public static IServiceCollection AddGraphQLPresentation(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddAuthorization()
            .AddInMemorySubscriptions()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<SubscriptionType>()
            .AddType<QueryType>()
            .AddType<MutationType>()
            .AddType<ActivityType>()
            .AddType<UpdateActivityInputType>()
            .ModifyOptions(_ => _.DefaultBindingBehavior = BindingBehavior.Explicit)
            .AddDiagnosticEventListener<ErrorLogger>()
            .AddErrorFilter<ErrorFilter>();

        services
            .AddActivityWatch()
            .AddPollWatch();

        return services;
    }
}
