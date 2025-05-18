namespace Meets.Scheduler.Activities;

internal sealed record ActivityChangedTopicEvent
(
    Guid Id,
    int? Version
)
{
    public static string GetTopicName(Guid id) => $"Activity:{id}:Changed";
}
