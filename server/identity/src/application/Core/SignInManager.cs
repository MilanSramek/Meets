using Meets.Identity.ApplicationUsers;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

using System.Collections.Frozen;
using System.Security.Claims;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Meets.Identity.Core;

internal sealed class SignInManager : SignInManager<ApplicationUser>
{
    private readonly FrozenSet<string> _scopes = FrozenSet.ToFrozenSet(
    [
        Scopes.OpenId,
        Scopes.Email,
        Scopes.Profile,
        Scopes.Roles
    ]);

    public SignInManager(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation)
            : base(
                userManager,
                contextAccessor,
                claimsFactory,
                optionsAccessor,
                logger,
                schemes,
                confirmation)
    {
        AuthenticationScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
    }

    public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(
        ApplicationUser user)
    {
        var request = Context.GetOpenIddictServerRequest()
           ?? throw new Exception("OpenIddict is not configured.");

        var principal = await base.CreateUserPrincipalAsync(user);
        var identity = (ClaimsIdentity)principal.Identity!;

        identity.SetScopes(_scopes.Intersect(request.GetScopes()));
        identity.SetDestinations(GetDestinations);

        return principal;
    }

    private IEnumerable<string> GetDestinations(Claim claim)
    {
        var identityOptions = Options.ClaimsIdentity!;

        if (claim.Type == identityOptions.UserNameClaimType)
        {
            yield return Destinations.AccessToken;

            if (claim.Subject?.HasScope(Scopes.Profile) == true)
            {
                yield return Destinations.IdentityToken;
            }

            yield break;
        }

        if (claim.Type == identityOptions.EmailClaimType)
        {
            yield return Destinations.AccessToken;

            if (claim.Subject?.HasScope(Scopes.Email) == true)
            {
                yield return Destinations.IdentityToken;
            }

            yield break;
        }

        if (claim.Type == identityOptions.RoleClaimType)
        {
            yield return Destinations.AccessToken;

            if (claim.Subject?.HasScope(Scopes.Roles) == true)
            {
                yield return Destinations.IdentityToken;
            }

            yield break;
        }

        if (claim.Type == Options.ClaimsIdentity.SecurityStampClaimType)
        {
            yield break;
        }

        yield return Destinations.AccessToken;
    }
}