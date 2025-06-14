using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

using Meets.Identity.Users;
using Microsoft.AspNetCore.Http;

namespace Meets.Identity.LogOutEndpoints;

public static class LogOutEndpoints
{
    public static void MapLogOutEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(EndpointPath.Logout, (
            [FromServices] ISignOutService signOutService,
            CancellationToken cancellationToken) =>
        {
            return signOutService.SignOutAsync(cancellationToken);
        })
        .WithSummary("Log out a user")
        .WithTags("Logout")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();
    }
}
