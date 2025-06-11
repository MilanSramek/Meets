using Microsoft.Extensions.Hosting;

namespace Meets.Common.Infrastructure.Hosting;

public static class HostEnvironmentExtensions
{
    public static bool IsDevelopmentStandAlone(
        this IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);

        return hostEnvironment.IsEnvironment(Environments.DevelopmentStandAlone);
    }

    public static bool IsOneOfDevelopment(
        this IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);

        return hostEnvironment.IsDevelopment()
            || hostEnvironment.IsDevelopmentStandAlone();
    }
}