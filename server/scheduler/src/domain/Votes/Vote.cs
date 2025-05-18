using Meets.Common.Expansions.Collections;

namespace Meets.Scheduler.Votes;

public sealed class Vote : AggregateRoot<Guid>,
    IWithVersion,
    ICreatedIntegrationEventSource,
    IChangedIntegrationEventSource,
    IDeletedIntegrationEventSource
{
    private bool _changed;
#pragma warning disable IDE0044 // Add readonly modifier
    private HashSet<VoteItem> _items;
#pragma warning restore IDE0044 // Add readonly modifier

    public Vote(Guid pollId)
    {
        PollId = pollId;

        _changed = true;
        _items = [];
    }

    public Vote(Guid id, Guid pollId) : this(pollId)
    {
        Id = id;
    }

    public Guid PollId { get; private set; }

    public IReadOnlyCollection<VoteItem> Items => _items;

    public int Version { get; private set; }

    public Vote SetItems(IEnumerable<VoteItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        var newItems = items.EvaluateToReadOnly();
        if (_items.SetEquals(newItems))
        {
            return this;
        }

        _items.Clear();
        _items.AddRange(newItems);

        SetChanged();
        return this;
    }

    public object GetCreatedEvent() => new VoteCreatedEvent(Id);
    public object GetChangedEvent() => new VoteChangedEvent(Id, Version);
    public object GetDeletedEvent() => new VoteDeletedEvent(Id);

    private void SetChanged()
    {
        if (!_changed)
        {
            _changed = true;
            Version++;
        }
    }

}
