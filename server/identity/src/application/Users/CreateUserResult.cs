using Meets.Common.Expansions.Collections;

using Microsoft.AspNetCore.Identity;

namespace Meets.Identity.Users;

public sealed class CreateUserResult
{
    public bool Succeeded { get; private init; }

    public bool IsConflicted { get; private init; }

    public bool HasErrors => Errors is { Count: > 0 };

    public IReadOnlyCollection<IdentityError> Errors { get; private init; }

    public static readonly CreateUserResult Success = new()
    {
        Succeeded = true,
        IsConflicted = false,
        Errors = []
    };

    public static readonly CreateUserResult Conflict = new()
    {
        Succeeded = false,
        IsConflicted = true,
        Errors = []
    };

    public static CreateUserResult WithIdentityErrors(
        IEnumerable<IdentityError> errors)
    {
        return new CreateUserResult
        {
            Succeeded = false,
            IsConflicted = false,
            Errors = errors.EvaluateToReadOnly()
        };
    }
}