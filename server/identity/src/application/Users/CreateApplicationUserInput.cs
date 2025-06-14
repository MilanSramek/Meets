namespace Meets.Identity.Users;

public sealed record CreateUserInput
(
    string UserName,
    string Password
);