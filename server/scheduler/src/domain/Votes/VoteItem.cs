namespace Meets.Scheduler.Votes;

public sealed record VoteItem
(
    DateOnly Date,
    VoteItemStatement Statement
);
