using Meets.Common.Domain;

using System.Diagnostics.CodeAnalysis;

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
    public string PasswordHash { get; private set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    public string SecurityStamp { get; private set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public string ConcurrencyStamp { get; private set; }

    public User(string userName)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        NormalizedUserName = NormalizeUserName(userName);

        SecurityStamp = Guid.NewGuid().ToString();
        UpdateConcurrencyStamp();
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
        NormalizedEmail = NormalizeEmail(value);
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

    [return: NotNullIfNotNull(nameof(value))]
    public static string? NormalizeEmail(string? value)
    {
        return value is { }
            ? value.Normalize().ToLowerInvariant()
            : null;
    }
}
