using Microsoft.AspNetCore.Builder;

namespace Meets.Common.Infrastructure.Identity;

public static class PipelineExtensions
{
    public static IApplicationBuilder UseIdentityContext(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<IdentityContextMiddleware>();
    }
}