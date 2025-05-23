using Meets.Common.Domain;

namespace Meets.Identity.ApplicationUsers;

public class ApplicationUser : AggregateRoot<Guid>
{
    public string UserName { get; private set; }

    public string NormalizedUserName { get; private set; }

    public string? Email { get; private set; }

    public ApplicationUser()
    {
    }

    public ApplicationUser(string userName)
    {
        SetUserName(userName);
        SetNormalizedUserName(userName);
    }

    public void SetUserName(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        UserName = value;
    }

    public void SetNormalizedUserName(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        NormalizedUserName = value.ToUpperInvariant();
    }
}
