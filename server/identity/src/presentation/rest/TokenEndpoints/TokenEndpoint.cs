using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;

using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

using Meets.Identity.Users;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Meets.Identity.TokenEndpoints;

internal static class TokenEndpoint
{
    public static void MapTokenEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(EndpointPath.Token, async (
            HttpContext context,
            [FromServices] ISignInService signInManager,
            CancellationToken cancellationToken) =>
        {
            var request = context.GetOpenIddictServerRequest();
            if (request is null || !request.IsPasswordGrantType())
            {
                return Results.BadRequest("Invalid grant type.");
            }

            var result = await signInManager.PasswordSignInAsync(new
            (
                UserName: request.Username!,
                Password: request.Password!
            ),
            cancellationToken);
            if (result.Succeeded)
            {
                return Results.Empty;
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
        })
        .Accepts<TokenEndpointRequest>("application/x-www-form-urlencoded")
        .WithSummary("Sign in with username and password")
        .WithTags("Authentication")
        .Produces<TokenEndpointResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces<AuthenticationProperties>(StatusCodes.Status403Forbidden);
    }
}
