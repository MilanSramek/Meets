namespace Meets.Scheduler.Happenings;

public sealed record HappeningModel
(
    Guid Id,
    string Name,
    string? Description,
    int Version
);
