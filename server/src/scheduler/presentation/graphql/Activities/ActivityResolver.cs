using Meets.Common.Domain;
using Meets.Scheduler.Activities;
using Meets.Scheduler.Polls;

namespace Meets.Scheduler.Activities;

internal sealed class ActivityResolver
{
    public async Task<PollModel> GetPollAsync(
        [Parent] ActivityModel activity,
        [Service] IReadOnlyRepository<Poll, Guid> polls,
        CancellationToken cancellationToken)
    {
        var poll = await polls
            .FirstAsync(_ => _.ActivityId == activity.Id, cancellationToken);// ToDo: DataLoader
        return poll.MapToModel();
    }
}
