using Microsoft.AspNetCore.Identity;

namespace Meets.Identity.Users;

public interface ISignInService
{
    public Task<SignInResult> PasswordSignInAsync(
        SignInUserInput input,
        CancellationToken cancellationToken);
}