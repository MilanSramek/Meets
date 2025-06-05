namespace Meets.Scheduler.Activities;

internal sealed record CreateActivityInput
(
    string Name,
    string? Description
);