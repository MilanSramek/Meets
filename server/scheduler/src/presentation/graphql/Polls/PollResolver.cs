using Meets.Common.Domain;
using Meets.Scheduler.Votes;

namespace Meets.Scheduler.Polls;

internal sealed class PollResolver
{
    public async Task<IEnumerable<VoteModel>> GetVotesAsync(
        [Parent] PollModel poll,
        [Service] IReadOnlyRepository<Vote, Guid> votes,
        CancellationToken cancellationToken)
    {
        var pollVotes = await votes
            .Where(_ => _.PollId == poll.Id)
            .ToListAsync(cancellationToken);// ToDo: DataLoader
        return pollVotes.MapToModel();
    }
}
