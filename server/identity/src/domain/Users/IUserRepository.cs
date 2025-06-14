using Meets.Common.Domain;

namespace Meets.Identity.Users;

public interface IUserRepository :
    IInsertRepository<User, Guid>,
    IUpdateRepository<User, Guid>,
    IDeleteRepository<User, Guid>
{
}
