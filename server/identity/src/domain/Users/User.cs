using Meets.Common.Domain;

namespace Meets.Identity.Users;

public class User : AggregateRoot<Guid>
{
    public string UserName { get; private set; }

    public string NormalizedUserName { get; private set; }

    public string? Email { get; private set; }

    public string? NormalizedEmail { get; private set; }

    /// <summary>
    /// Gets or sets a salted and hashed representation of the password for this user.
    /// </summary>
    public string? PasswordHash { get; private set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    public string SecurityStamp { get; private set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public string ConcurrencyStamp { get; private set; }

    public string? Name { get; private set; }

    public User(string userName)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        NormalizedUserName = NormalizeUserName(userName);

        SecurityStamp = Guid.NewGuid().ToString();
        UpdateConcurrencyStamp();
    }

    public void SetName(string? name)
    {
        Name = name;
    }

    public void SetPasswordHash(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        PasswordHash = value;
        SecurityStamp = Guid.NewGuid().ToString();
        UpdateConcurrencyStamp();
    }

    public void SetEmail(string? value)
    {
        Email = value;
        NormalizedEmail = value is { }
            ? NormalizeEmail(value)
            : null;
        UpdateConcurrencyStamp();
    }

    private void UpdateConcurrencyStamp()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    public static string NormalizeUserName(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Normalize().ToLowerInvariant();
    }

    public static string NormalizeEmail(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Normalize().ToLowerInvariant();
    }
}
