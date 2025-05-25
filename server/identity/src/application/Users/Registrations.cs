using Microsoft.Extensions.DependencyInjection;

namespace Meets.Identity.Users;

public static class Registrations
{
    public static IServiceCollection AddUsers(
        this IServiceCollection services)
    {
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<ISignOutService, SignOutService>();
        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}