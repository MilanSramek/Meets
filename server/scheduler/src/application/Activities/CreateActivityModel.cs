namespace Meets.Scheduler.Activities;

public sealed record CreateActivityModel
(
    string Name,
    string? Description,
    Guid? OwnerId
);
