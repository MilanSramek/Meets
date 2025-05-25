using Meets.Common.Domain;

using Microsoft.AspNetCore.Identity;

namespace Meets.Identity.Users;

internal sealed class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public AccountService(
        UserManager<User> userManager,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _userManager = userManager;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task<CreateUserResult> CreateUserAsync(
        CreateUserInput input,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input);

        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();

        var existingUser = await _userManager.FindByNameAsync(input.UserName);
        if (existingUser is { })
        {
            return CreateUserResult.Conflict;
        }

        var user = new User(input.UserName);
        var result = await _userManager.CreateAsync(user, input.Password);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return result.Succeeded
            ? CreateUserResult.Success
            : CreateUserResult.WithIdentityErrors(result.Errors);
    }
}