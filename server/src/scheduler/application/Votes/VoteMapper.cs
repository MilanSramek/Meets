namespace Meets.Scheduler.Votes;

public static class VoteMapper
{
    public static VoteModel MapToModel(this Vote vote)
    {
        return new VoteModel(
            vote.Id,
            vote.PollId,
            vote.Items.MapToModels());
    }

    public static IEnumerable<VoteModel> MapToModel(this IEnumerable<Vote> votes) => votes
        .Select(vote => vote.MapToModel());
}
