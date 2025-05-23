using Meets.Common.Domain;

namespace Meets.Identity.ApplicationUsers;

public interface IApplicationUserRepository :
    IInsertRepository<ApplicationUser, Guid>,
    IUpdateRepository<ApplicationUser, Guid>,
    IDeleteRepository<ApplicationUser, Guid>
{
}
