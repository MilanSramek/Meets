using Meets.Common.Presentation.GraphQL;
using Meets.Scheduler.Activities;
using Meets.Scheduler.Votes;

namespace Meets.Scheduler;

internal sealed class Mutation
{
    public Task<ActivityModel> CreateActivityAsync(
        CreateActivityInput input,
        [Service] IActivityCreationService activityService,
        CancellationToken cancellationToken)
    {
        return activityService.CreateActivityAsync(input, cancellationToken);
    }

    public Task<ActivityModel> UpdateActivityAsync(
        Guid id,
        UpdateActivityInterInput input,
        [Service] IActivityUpdateService activityService,
        CancellationToken cancellationToken)
    {
        var properInput = new UpdateActivityInput(
            input.Name.ToOpt(),
            input.Description.ToOpt());
        return activityService.UpdateActivityAsync(id, properInput, cancellationToken);
    }

    public Task<VoteModel> AddVoteAsync(
        CreateVoteInput input,
        [Service] IVoteCreationService voteService,
        CancellationToken cancellationToken)
    {
        return voteService.CreateVoteAsync(input, cancellationToken);
    }

    public Task<VoteModel> UpdateVoteAsync(
        Guid id,
        IEnumerable<CreateUpdateVoteItemInput> items,
        [Service] IVoteUpdateService voteService,
        CancellationToken cancellationToken)
    {
        return voteService.UpdateVoteAsync(id, items, cancellationToken);
    }
}
