using CSharpFunctionalExtensions;

using System.Security.Claims;

namespace Meets.Common.Infrastructure.Identity;

public static class ClaimsPrincipalExtensions
{
    public static Result<Guid,(string Message, object? Data)> GetUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        if (principal.Identity?.IsAuthenticated != true)
        {
            return Result.Failure<Guid,(string,object?)>(("User is not authenticated.", null));
        }

        var userIdClaim = principal.FindFirst(IdentityClaims.UserId);
        if (userIdClaim is null)
        {
            return Result.Failure<Guid,(string,object?)>(("User ID claim is missing.", null));
        }

        if (Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        return Result.Failure<Guid,(string,object?)>((
            "User ID claim is invalid: {userIdClaim.Value}",
            userIdClaim.Value));
    }
}
