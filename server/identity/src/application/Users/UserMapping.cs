namespace Meets.Identity.Users;

public static class UserMapping
{
    public static UserModel MapToModel(this User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserModel(
            user.Id,
            user.UserName,
            user.Email);
    }
}