using Microsoft.Extensions.DependencyInjection;

namespace Meets.Identity;

public static class Registrations
{
    public static IServiceCollection AddRestPresentation(
        this IServiceCollection services)
    {
        services
            .AddOpenIddictServer()
            .AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "Meets Identity API",
                        Version = "v1",
                        Description = "API for managing user identities."
                    };
                    return Task.CompletedTask;
                });
            });

        return services;
    }

    private static IServiceCollection AddOpenIddictServer(
        this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddServer(options =>
            {
                options.UseAspNetCore()
                    .DisableTransportSecurityRequirement()
                    .EnableTokenEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough();

                options
                    .SetTokenEndpointUris(EndpointPath.Token)
                    .SetEndSessionEndpointUris(EndpointPath.Logout);
            });

        return services;
    }
}
