using Microsoft.AspNetCore.Routing;

using Meets.Identity.TokenEndpoints;
using Meets.Identity.RegisterEndpoints;
using Meets.Identity.LogOutEndpoints;

namespace Meets.Identity;

public static class Endpoints
{
    public static void MapAuthorityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapTokenEndpoint();
        app.MapRegisterEndpoint();
        app.MapLogOutEndpoint();
    }
}
