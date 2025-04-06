namespace Meets.Scheduler.Votes;

public static class VoteItemMapper
{
    public static VoteItemModel MapToModel(this VoteItem voteItem) => new(
        voteItem.Date,
        voteItem.Statement);

    public static IEnumerable<VoteItemModel> MapToModels(this IEnumerable<VoteItem> voteItems) => voteItems
        .Select(voteItem => voteItem.MapToModel());
}
