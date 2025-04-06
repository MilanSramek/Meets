namespace Meets.Scheduler.Happenings;

public sealed record HappeningChangedEvent
(
    Guid Id,
    int Version
);
