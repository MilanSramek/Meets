using Meets.Common.Domain;
using Meets.Identity.ApplicationUsers;

using Microsoft.AspNetCore.Identity;

namespace Meets.Identity.Core;

internal sealed class UserStore : IUserStore<ApplicationUser>
{
    private readonly IApplicationUserRepository _repository;

    public UserStore(IApplicationUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user,
        CancellationToken cancellationToken)
    {
        await _repository.InsertAsync(cancellationToken, user);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user,
        CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(cancellationToken, user);
        return IdentityResult.Success;
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId,
        CancellationToken cancellationToken)
    {
        return await _repository.GetAsync(Guid.Parse(userId), cancellationToken);
    }

    public Task<ApplicationUser?> FindByNameAsync(string normalizedUserName,
        CancellationToken cancellationToken)
    {
        return _repository
            .FirstOrDefaultAsync(
                _ => _.NormalizedUserName == normalizedUserName,
                cancellationToken);
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user,
        CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(cancellationToken, user);
        return IdentityResult.Success;
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user,
        CancellationToken _)
    {
        return Task.FromResult<string?>(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(ApplicationUser user,
        CancellationToken _)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user,
        CancellationToken _)
    {
        return Task.FromResult<string?>(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user,
        string? normalizedName, CancellationToken _)
    {
        user.SetNormalizedUserName(normalizedName
            ?? throw new InvalidOperationException("Normalized user name cannot be null."));
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(ApplicationUser user,
        string? userName,
        CancellationToken _)
    {
        user.SetUserName(userName
            ?? throw new InvalidOperationException("User name cannot be null."));
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}