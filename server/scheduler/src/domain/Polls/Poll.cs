namespace Meets.Scheduler.Polls;

public sealed class Poll : AggregateRoot<Guid>
{
    public Guid ActivityId { get; private set; }

    public Poll(Guid activityId)
    {
        ActivityId = activityId;
    }

    public Poll(Guid id, Guid activityId) : this(activityId)
    {
        Id = id;
    }
}
