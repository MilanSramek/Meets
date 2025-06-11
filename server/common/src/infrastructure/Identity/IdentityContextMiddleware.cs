using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Meets.Common.Infrastructure.Identity;

internal sealed class IdentityContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;

    public IdentityContextMiddleware(
        RequestDelegate next,
        ILoggerFactory loggerFactory)
    {
        _next = next;
        _loggerFactory = loggerFactory;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        if (httpContext.User.Identity is { IsAuthenticated: true })
        {
            var identityContextAccessor = httpContext.RequestServices
                .GetRequiredService<IIdentityContextAccessor>();

            identityContextAccessor.Context = new IdentityContext(
                httpContext.User,
                _loggerFactory.CreateLogger<IdentityContext>());
        }

        await _next(httpContext);
    }
}