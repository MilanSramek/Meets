using Meets.Common.Presentation.GraphQL;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Identity;

public static class Registrations
{
    public static IServiceCollection AddGraphQLPresentation(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddAuthorization()
            .AddInMemorySubscriptions()
            .AddQueryType<Query>()
            // .AddMutationType<Mutation>()
            // .AddSubscriptionType<SubscriptionType>()
            .AddType<QueryType>()
            // .AddType<MutationType>()
            .ModifyOptions(_ => _.DefaultBindingBehavior = BindingBehavior.Explicit)
            .AddDiagnosticEventListener<ErrorLogger>()
            .AddErrorFilter<ErrorFilter>();

        return services;
    }
}
