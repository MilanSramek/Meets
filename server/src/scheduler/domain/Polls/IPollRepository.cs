using Meets.Common.Domain;

namespace Meets.Scheduler.Polls;

public interface IPollRepository :
    IInsertRepository<Poll, Guid>,
    IUpdateRepository<Poll, Guid>
{
}
