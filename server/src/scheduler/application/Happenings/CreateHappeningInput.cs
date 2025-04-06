namespace Meets.Scheduler.Happenings;

public sealed record CreateHappeningInput
(
    string Name,
    string? Description
);
