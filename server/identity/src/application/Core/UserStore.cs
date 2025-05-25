using Meets.Common.Domain;
using Meets.Identity.Users;

using Microsoft.AspNetCore.Identity;

namespace Meets.Identity.Core;

internal sealed class UserStore :
    IUserStore<User>,
    IUserPasswordStore<User>,
    IUserEmailStore<User>
{
    private readonly IUserRepository _repository;

    public UserStore(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IdentityResult> CreateAsync(User user,
        CancellationToken cancellationToken)
    {
        await _repository.InsertAsync(cancellationToken, user);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(User user,
        CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(cancellationToken, user);
        return IdentityResult.Success;
    }

    public async Task<User?> FindByIdAsync(string userId,
        CancellationToken cancellationToken)
    {
        return await _repository.GetAsync(Guid.Parse(userId), cancellationToken);
    }

    public Task<User?> FindByNameAsync(string normalizedUserName,
        CancellationToken cancellationToken)
    {
        return _repository
            .FirstOrDefaultAsync(
                _ => _.NormalizedUserName == normalizedUserName,
                cancellationToken);
    }

    public async Task<IdentityResult> UpdateAsync(User user,
        CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(cancellationToken, user);
        return IdentityResult.Success;
    }

    public Task<string?> GetNormalizedUserNameAsync(User user,
        CancellationToken _)
    {
        return Task.FromResult<string?>(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(User user,
        CancellationToken _)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(User user,
        CancellationToken _)
    {
        return Task.FromResult<string?>(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(User _1, string? _2,
        CancellationToken _3)
    {
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(User _1, string? _2, CancellationToken _3)
    {
        throw new InvalidOperationException("User name cannot be changed.");
    }

    public Task SetPasswordHashAsync(
        User user,
        string? passwordHash,
        CancellationToken _)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new InvalidOperationException("Password hash cannot be null or empty.");
        }
        user.SetPasswordHash(passwordHash);

        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(
        User user,
        CancellationToken _)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult<string?>(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(
        User user,
        CancellationToken _)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.PasswordHash is { });
    }

    public Task SetEmailAsync(User user, string? email, CancellationToken _)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.SetEmail(email);
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(User user, CancellationToken _)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken _)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken _)
    {
        throw new NotImplementedException();
    }

    public Task<User?> FindByEmailAsync(string normalizedEmail,
        CancellationToken cancellationToken)
    {
        return _repository
            .FirstOrDefaultAsync(
                _ => _.NormalizedEmail == normalizedEmail,
                cancellationToken);
    }

    public Task<string?> GetNormalizedEmailAsync(User user, CancellationToken _)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(User _1, string? _2,
        CancellationToken _3)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}