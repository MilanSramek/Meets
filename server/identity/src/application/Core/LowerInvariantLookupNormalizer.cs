using Meets.Identity.Users;

using Microsoft.AspNetCore.Identity;

using System.Diagnostics.CodeAnalysis;

namespace Meets.Identity.Core;

/// <summary>
/// Implements <see cref="ILookupNormalizer"/> by converting keys to their upper cased invariant culture representation.
/// </summary>
internal sealed class UserLookupNormalizer : ILookupNormalizer
{
    public static readonly UserLookupNormalizer Instance = new();

    [return: NotNullIfNotNull(nameof(name))]
    public string? NormalizeName(string? name)
    {
        return name is { }
            ? User.NormalizeUserName(name)
            : null;
    }

    [return: NotNullIfNotNull(nameof(email))]
    public string? NormalizeEmail(string? email) => email is { }
        ? User.NormalizeEmail(email)
        : null;
}
