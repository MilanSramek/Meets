namespace Meets.Scheduler.Activities;

public sealed record ActivityModel
(
    Guid Id,
    string Name,
    string? Description,
    int Version
);
