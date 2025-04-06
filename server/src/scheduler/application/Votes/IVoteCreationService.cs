namespace Meets.Scheduler.Votes;

public interface IVoteCreationService
{
    public Task<VoteModel> CreateVoteAsync(
        CreateVoteInput input,
        CancellationToken cancellationToken);
}
