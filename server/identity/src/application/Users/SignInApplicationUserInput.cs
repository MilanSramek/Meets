namespace Meets.Identity.Users;

public sealed record SignInUserInput
(
    string UserName,
    string Password
);