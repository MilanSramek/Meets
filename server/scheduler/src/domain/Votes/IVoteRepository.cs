namespace Meets.Scheduler.Votes;

public interface IVoteRepository :
    IInsertRepository<Vote, Guid>,
    IUpdateRepository<Vote, Guid>
{
}
