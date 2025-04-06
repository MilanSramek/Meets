namespace Meets.Scheduler.Votes;

public sealed record VoteItemModel(
    DateOnly Date,
    VoteItemStatement Statement);
