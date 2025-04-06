namespace Meets.Scheduler.Polls;

public sealed class Poll : AggregateRoot<Guid>
{
    public Guid HappeningId { get; private set; }

    public Poll(Guid eventId)
    {
        HappeningId = eventId;
    }

    public Poll(Guid id, Guid happeningId) : this(happeningId)
    {
        Id = id;
    }
}
