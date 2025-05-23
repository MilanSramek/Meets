namespace Meets.Identity;

public interface ISignOutService
{
    public Task SignOutAsync(CancellationToken cancellationToken);
}