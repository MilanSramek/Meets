namespace Meets.Scheduler.Activities;

public sealed record CreateActivityInput
(
    string Name,
    string? Description,
    Guid? OwnerId
);
