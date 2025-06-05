namespace Meets.Common.Domain;

public interface IIdentityContext
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// </summary>
    public Guid? UserId { get; }
}