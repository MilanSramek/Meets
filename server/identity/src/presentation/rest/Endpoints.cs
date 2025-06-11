using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

using Meets.Identity.Users;
using Meets.Identity.TokenEndpoints;
using Meets.Identity.RegisterEndpoints;

namespace Meets.Identity;

public static class Endpoints
{
    public static void MapAuthorityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapTokenEndpoint();
        app.MapRegisterEndpoint();

        app.MapPost(EndpointPath.Logout, (
            [FromServices] ISignOutService signOutService,
            CancellationToken cancellationToken) =>
        {
            return signOutService.SignOutAsync(cancellationToken);
        })
        .RequireAuthorization();
    }
}
