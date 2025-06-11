namespace Meets.Identity.Users;

public sealed record UserModel
(
    Guid Id,
    string UserName,
    string? Email,
    string? Name
);