using Meets.Common.Presentation.GraphQL;
using Meets.Scheduler.Activities;
using Meets.Scheduler.Votes;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Meets.Scheduler;

internal sealed class Mutation
{
    public async Task<ActivityModel> CreateActivityAsync(
        CreateActivityRequest request,
        [Service] IHttpContextAccessor httpContextAccessor,
        [Service] IActivityCreationService activityService,
        CancellationToken cancellationToken)
    {
        CreateActivityInput input = new(
            request.Name,
            request.Description,
            null);
        // ToDo: IdentityContext
        var ctx = httpContextAccessor.HttpContext;
        if (ctx?.Request.Headers.TryGetValue("Authorization", out var _) == true)
        {
            var result = await ctx.AuthenticateAsync();
            string? rawOwnerId = result.Principal?.FindFirst("sub")?.Value;

            if (!result.Succeeded
                || rawOwnerId is null
                || !Guid.TryParse(rawOwnerId, out var ownerId))
            {
                throw new GraphQLException(ErrorBuilder.New()
                    .SetMessage("You are not allowed to access this resource.")
                    .SetCode("FORBIDDEN")
                    .Build());
            }

            input = input with
            {
                OwnerId = ownerId
            };
        }

        return await activityService.CreateActivityAsync(input, cancellationToken);
    }

    public Task<ActivityModel> UpdateActivityAsync(
        Guid id,
        UpdateActivityRequest request,
        [Service] IActivityUpdateService activityService,
        CancellationToken cancellationToken)
    {
        var properInput = new UpdateActivityInput(
            request.Name.ToOpt(),
            request.Description.ToOpt());
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
