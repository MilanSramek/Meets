using Meets.Common.Application.Identity;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Common.Infrastructure.Identity;

internal static class Registrations
{
    public static IServiceCollection AddIdentityContext(
        this IServiceCollection services)
    {
        services.AddSingleton<IIdentityContextAccessor, IdentityContextAccessor>();
        return services.AddSingleton(provider =>
        {
            var accessor = provider.GetRequiredService<IIdentityContextAccessor>();
            return accessor.Context ?? throw new UnauthenticatedException();
        });
    }
}