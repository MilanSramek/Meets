namespace Meets.Scheduler.Polls;

public sealed record PollChangedTopicEvent
(
    Guid Id,
    int? Version
)
{
    public static string GetTopicName(Guid id) => $"Poll:{id}:Changed";
}
