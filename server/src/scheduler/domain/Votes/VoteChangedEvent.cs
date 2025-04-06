namespace Meets.Scheduler.Votes;

public sealed record VoteChangedEvent
(
    Guid Id,
    int Version
);
