using Meets.Identity.ApplicationUsers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using OpenIddict.Abstractions;

using System.Security.Claims;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Meets.Identity.Core;

internal sealed class UserClaimsPrincipalFactory :
    IUserClaimsPrincipalFactory<ApplicationUser>
{
    private readonly ClaimsIdentityOptions _options;

    public UserClaimsPrincipalFactory(
        IOptions<IdentityOptions> optionsAccessor)
    {
        _options = optionsAccessor.Value.ClaimsIdentity;
    }

    public Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role);

        identity
            .SetClaim(_options.UserIdClaimType, user.Id.ToString())
            .SetClaim(_options.EmailClaimType, user.Email)
            .SetClaim(_options.UserNameClaimType, user.UserName);

        return Task.FromResult(new ClaimsPrincipal(identity));
    }
}