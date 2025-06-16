namespace Meets.Identity.Users;

public sealed record CreateUserInput
(
    string Username,
    string Password
);