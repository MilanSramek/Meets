using Meets.Common.Domain;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Meets.Common.Infrastructure;

internal sealed class IdentityContext : IIdentityContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public Guid? UserId
    {
        get
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx?.User.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            if (ctx?.Request.Headers.TryGetValue("Authorization", out var _) == true)
            {
                var result = await ctx.AuthenticateAsync();
                string? rawOwnerId = result.Principal?.FindFirst("sub")?.Value;

                if (!result.Succeeded
                    || rawOwnerId is null
                    || !Guid.TryParse(rawOwnerId, out var ownerId))
                {
                    throw new GraphQLException(ErrorBuilder.New()
                        .SetMessage("You are not allowed to access this resource.")
                        .SetCode("FORBIDDEN")
                        .Build());
                }

                input = input with
                {
                    OwnerId = ownerId
                };
            }
        }
    }
}