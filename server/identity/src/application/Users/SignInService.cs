using Meets.Common.Domain;
using Meets.Identity.Core;

using Microsoft.AspNetCore.Identity;

namespace Meets.Identity.Users;

internal sealed class SignInService : ISignInService
{
    private readonly SignInManager _signInManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public SignInService(
        SignInManager signInManager,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _signInManager = signInManager;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task<SignInResult> PasswordSignInAsync(
        SignInUserInput input,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input);

        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();
        var result = await _signInManager.PasswordSignInAsync(
            input.UserName,
            input.Password,
            isPersistent: true,
            lockoutOnFailure: false);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return result;
    }
}