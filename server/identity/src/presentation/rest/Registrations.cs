using Microsoft.Extensions.DependencyInjection;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Meets.Identity;

public static class Registrations
{
    public static IServiceCollection AddRestPresentation(
        this IServiceCollection services)
    {
        services
            .AddOpenIddictServer();

        return services;
    }

    private static IServiceCollection AddOpenIddictServer(
        this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddServer(options =>
            {
                options
                    .UseAspNetCore()
                    .DisableTransportSecurityRequirement()
                    .EnableTokenEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough();

                options
                    .SetTokenEndpointUris(EndpointPath.Token)
                    .SetEndSessionEndpointUris(EndpointPath.Logout);

                options
                    .RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                options
                    .AllowPasswordFlow();
                options
                    .AcceptAnonymousClients();

                options
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
            });

        return services;
    }
}
