using Microsoft.AspNetCore.Identity;

namespace Meets.Identity;

public interface ISignInService
{
    public Task<SignInResult> PasswordSignInAsync(
        string userName,
        string password,
        CancellationToken cancellationToken);
}