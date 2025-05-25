namespace Meets.Identity.Users;

public interface ISignOutService
{
    public Task SignOutAsync(CancellationToken cancellationToken);
}