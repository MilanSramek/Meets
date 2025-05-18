namespace Meets.Scheduler.Activities;

public sealed record ActivityChangedEvent
(
    Guid Id,
    int Version
);
