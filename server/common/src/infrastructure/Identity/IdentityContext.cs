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

            var userId = _principal.GetUserId();
            if (userId.IsSuccess)
            {
                _userId = userId.Value;
                return _userId;
            }

            var error = userId.Error;
            _logger.LogWarning(error.Message, error.Data);
            return null;
        }
    }
}
