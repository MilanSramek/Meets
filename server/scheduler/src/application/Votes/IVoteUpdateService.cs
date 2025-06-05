namespace Meets.Scheduler.Votes;

public interface IVoteUpdateService
{
    public Task<VoteModel> UpdateVoteAsync(Guid id, IEnumerable<CreateUpdateVoteItemModel> items,
        CancellationToken cancellationToken);
}
