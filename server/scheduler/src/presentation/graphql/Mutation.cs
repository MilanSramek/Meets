using Meets.Common.Application.Identity;
using Meets.Common.Presentation.GraphQL;
using Meets.Scheduler.Activities;
using Meets.Scheduler.Votes;

namespace Meets.Scheduler;

internal sealed class Mutation
{
    public async Task<ActivityModel> CreateActivityAsync(
        CreateActivityInput request,
        [Service] IIdentityContext identityContext,
        [Service] IActivityCreationService activityService,
        CancellationToken cancellationToken)
    {
        CreateActivityModel model = new(
            request.Name,
            request.Description,
            identityContext.UserId);

        return await activityService.CreateActivityAsync(model, cancellationToken);
    }

    public Task<ActivityModel> UpdateActivityAsync(
        Guid id,
        UpdateActivityInput request,
        [Service] IActivityUpdateService activityService,
        CancellationToken cancellationToken)
    {
        var properInput = new UpdateActivityModel(
            request.Name.ToOpt(),
            request.Description.ToOpt());
        return activityService.UpdateActivityAsync(id, properInput, cancellationToken);
    }

    public Task<VoteModel> AddVoteAsync(
        CreateVoteModel input,
        [Service] IVoteCreationService voteService,
        CancellationToken cancellationToken)
    {
        return voteService.CreateVoteAsync(input, cancellationToken);
    }

    public Task<VoteModel> UpdateVoteAsync(
        Guid id,
        IEnumerable<CreateUpdateVoteItemModel> items,
        [Service] IVoteUpdateService voteService,
        CancellationToken cancellationToken)
    {
        return voteService.UpdateVoteAsync(id, items, cancellationToken);
    }
}
