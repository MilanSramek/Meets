using Meets.Common.Application.Identity;

using Microsoft.AspNetCore.Http;

namespace Meets.Common.Infrastructure.Identity;

public static class HttpContextExtensions
{
    private const string IdentityContextKey = "IdentityContext";

    public static IIdentityContext? GetIdentityContext(
        this HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return context.Items.TryGetValue(IdentityContextKey, out object? value)
            ? (IIdentityContext?)value
            : default;
    }

    public static void SetIdentityContext(
        this HttpContext context,
        IIdentityContext? identityContext)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Items[IdentityContextKey] = identityContext;
    }
}