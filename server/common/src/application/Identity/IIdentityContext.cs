namespace Meets.Common.Application.Identity;

public interface IIdentityContext
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// </summary>
    public Guid? UserId { get; }
}