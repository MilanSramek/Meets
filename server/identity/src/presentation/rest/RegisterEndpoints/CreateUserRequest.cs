namespace Meets.Identity.RegisterEndpoints;

public sealed record CreateUserRequest
(
    string Username,
    string Password
);