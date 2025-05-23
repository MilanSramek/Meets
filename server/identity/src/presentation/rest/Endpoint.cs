using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;

using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Meets.Identity;

public static class Endpoints
{
    public static void MapAuthorityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(EndpointPath.Token, async (
            HttpContext context,
            ISignInService signInManager,
            CancellationToken cancellationToken) =>
        {
            var request = context.GetOpenIddictServerRequest();
            if (request is null || request.IsPasswordGrantType())
            {
                return Results.BadRequest("Invalid grant type.");
            }

            var result = await signInManager.PasswordSignInAsync(
                request.Username!,
                request.Password!,
                cancellationToken);
            if (result.Succeeded)
            {
                return Results.Ok();
            }

            return Results.Forbid(
                new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error]
                        = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription]
                        = result switch
                        {
                            { IsLockedOut: true } => "User is locked out.",
                            { IsNotAllowed: true } => "User is not allowed to sign in.",
                            { RequiresTwoFactor: true } => "Two factor authentication is required.",
                            _ => "Username or password is invalid.",
                        }
                }),
                [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]);
        });

        app.MapPost(EndpointPath.Logout, (
            ISignOutService signOutService,
            CancellationToken cancellationToken) =>
        {
            return signOutService.SignOutAsync(cancellationToken);
        });
    }
}