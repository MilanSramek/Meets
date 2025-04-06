using Meets.Common.Presentation.Graphql;
using Meets.Scheduler.Happenings;
using Meets.Scheduler.Votes;

namespace Meets.Scheduler;

internal sealed class Mutation
{
    public Task<HappeningModel> CreateHappeningAsync(
        CreateHappeningInput input,
        [Service] IHappeningCreationService happeningService,
        CancellationToken cancellationToken)
    {
        return happeningService.CreateEventAsync(input, cancellationToken);
    }

    public Task<HappeningModel> UpdateHappeningAsync(
        Guid id,
        UpdateHappeningInterInput input,
        [Service] IHappeningUpdateService happeningService,
        CancellationToken cancellationToken)
    {
        var properInput = new UpdateHappeningInput(
            input.Name.ToOpt(),
            input.Description.ToOpt());
        return happeningService.UpdateEventAsync(id, properInput, cancellationToken);
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
