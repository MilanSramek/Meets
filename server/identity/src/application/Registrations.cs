using Meets.Identity.Users;
using Meets.Identity.Core;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Meets.Identity;

public static class Registrations
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services
            .AddIdentity()
            .AddUsers();

        return services;
    }

    private static IServiceCollection AddIdentity(
        this IServiceCollection services)
    {
        services
          .AddIdentityCore<User>(options =>
          {
              options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
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