namespace Meets.Scheduler.Votes;

public sealed record CreateUpdateVoteItemModel(
    DateOnly Date,
    VoteItemStatement Statement);
