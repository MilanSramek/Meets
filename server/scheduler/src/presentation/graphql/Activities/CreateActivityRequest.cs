namespace Meets.Scheduler.Activities;

internal sealed record CreateActivityRequest
(
    string Name,
    string? Description
);