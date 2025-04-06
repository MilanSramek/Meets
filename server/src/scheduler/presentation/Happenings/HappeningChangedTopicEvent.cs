namespace Meets.Scheduler.Happenings;

internal sealed record HappeningChangedTopicEvent
(
    Guid Id,
    int? Version
)
{
    public static string GetTopicName(Guid id) => $"Happening:{id}:Changed";
}
