namespace Meets.Scheduler.Votes;

public sealed record CreateUpdateVoteItemInput(
    DateOnly Date,
    VoteItemStatement Statement);
