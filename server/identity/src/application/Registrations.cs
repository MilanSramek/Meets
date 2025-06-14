using Meets.Identity.Users;
using Meets.Identity.Core;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;
using OpenIddict.Abstractions;

namespace Meets.Identity;

public static class Registrations
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services
            .AddIdentity()
            .AddOpenIddictServer()
            .AddUsers();

        return services;
    }

    private static IServiceCollection AddOpenIddictServer(
        this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddServer(options => options
                .RegisterScopes(
                    Scopes.OpenId,
                    Scopes.OfflineAccess,
                    Scopes.Roles)
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .AcceptAnonymousClients()
                .DisableAccessTokenEncryption()
                .AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate()
                .AddEventHandler<ProcessSignInContext>(builder => builder
                    .UseSingletonHandler<SignInHandler>()));

        return services;
    }

    private static IServiceCollection AddIdentity(
        this IServiceCollection services)
    {
        services
            .AddIdentityCore<User>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
                options.ClaimsIdentity.EmailClaimType = Claims.Email;

                options.User.RequireUniqueEmail = false;

                options.Lockout.AllowedForNewUsers = false;

                // ToDo: Set proper options for signin
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // ToDo: Set proper options for password
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager>()
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
            .AddUserStore<UserStore>();

        services
            .AddSingleton<ILookupNormalizer, UserLookupNormalizer>();

        return services;
    }
}