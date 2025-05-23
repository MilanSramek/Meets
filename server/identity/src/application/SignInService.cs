using Meets.Common.Domain;
using Meets.Identity.Core;

using Microsoft.AspNetCore.Identity;

namespace Meets.Identity;

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
        string userName,
        string password,
        CancellationToken cancellationToken)
    {
        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();
        var result = await _signInManager.PasswordSignInAsync(
            userName,
            password,
            isPersistent: true,
            lockoutOnFailure: false);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return result;
    }
}