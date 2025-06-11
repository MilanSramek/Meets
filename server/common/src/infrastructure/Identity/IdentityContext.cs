using Meets.Common.Application.Identity;

using Microsoft.Extensions.Logging;

using System.Security.Claims;

namespace Meets.Common.Infrastructure.Identity;

internal sealed class IdentityContext : IIdentityContext
{
    private readonly ClaimsPrincipal _principal;
    private readonly ILogger<IdentityContext> _logger;

    private Guid? _userId;

    public IdentityContext(
        ClaimsPrincipal principal,
        ILogger<IdentityContext> logger)
    {
        _principal = principal ?? throw new ArgumentNullException(nameof(principal));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Guid? UserId
    {
        get
        {
            if (_userId.HasValue)
            {
                return _userId;
            }

            if (_principal.Identity?.IsAuthenticated != true)
            {
                _logger.LogDebug("User is not authenticated.");
                return null;
            }

            var userIdClaim = _principal.FindFirst(IdentityClaims.UserId);
            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID claim is missing.");
                return null;
            }

            if (Guid.TryParse(userIdClaim.Value, out var userId))
            {
                _userId = userId;
                return userId;
            }

            _logger.LogWarning("User ID claim is invalid: {UserIdClaimValue}",
                userIdClaim.Value);
            return null;
        }
    }
}