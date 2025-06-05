namespace Meets.Scheduler.Votes;

public interface IVoteCreationService
{
    public Task<VoteModel> CreateVoteAsync(
        CreateVoteModel input,
        CancellationToken cancellationToken);
}
