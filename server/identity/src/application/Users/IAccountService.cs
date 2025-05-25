namespace Meets.Identity.Users;

public interface IAccountService
{
    public Task<CreateUserResult> CreateUserAsync(
        CreateUserInput input,
        CancellationToken cancellationToken);
}