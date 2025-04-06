using Meets.Common.Domain;
using Meets.Scheduler.Polls;

namespace Meets.Scheduler.Happenings;

internal sealed class HappeningResolver
{
    public async Task<PollModel> GetPollAsync(
        [Parent] HappeningModel happening,
        [Service] IReadOnlyRepository<Poll, Guid> polls,
        CancellationToken cancellationToken)
    {
        var poll = await polls
            .FirstAsync(_ => _.HappeningId == happening.Id, cancellationToken);// ToDo: DataLoader
        return poll.MapToModel();
    }
}
