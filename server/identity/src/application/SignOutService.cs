using Meets.Common.Domain;
using Meets.Identity.Core;

namespace Meets.Identity;

internal sealed class SignOutService : ISignOutService
{
    private readonly SignInManager _signInManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public SignOutService(
        SignInManager signInManager,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _signInManager = signInManager;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task SignOutAsync(
        CancellationToken cancellationToken)
    {
        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();
        await _signInManager.SignOutAsync();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
    }
}