namespace Meets.Common.Infrastructure.Identity;

public static class IdentityClaims
{
    /// <summary>
    /// The claim type for the unique identifier of the user.
    /// </summary>
    public const string UserId = "sub";

    /// <summary>
    /// The claim type for the user's email address.
    /// </summary>
    public const string Email = "email";

    /// <summary>
    /// The claim type for the user's name.
    /// </summary>
    public const string Name = "name";

    /// <summary>
    /// The claim type for the user's roles.
    /// </summary>
    public const string Roles = "roles";
}