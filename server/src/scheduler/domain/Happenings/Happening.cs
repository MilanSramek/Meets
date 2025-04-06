namespace Meets.Scheduler.Happenings;

public sealed class Happening : AggregateRoot<Guid>, IWithVersion
{
    private bool _changed = false;

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public int Version { get; private set; }

    private Happening()
    {
    }

    internal Happening(string name)
    {
        SetName(name);
    }

    internal Happening(Guid id, string name) : this(name)
    {
        Id = id;
    }

    public Happening SetName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (string.IsNullOrWhiteSpace(name))
        {
            throw Error.NameIsRequired();
        }

        if (Name != name)
        {
            Name = name;
            SetChanged();
        }
        Name = name;
        return this;
    }

    public Happening SetDescription(string? description)
    {
        if (Description != description)
        {
            Description = description;
            SetChanged();
        }
        return this;
    }

    private void SetChanged()
    {
        if (!_changed)
        {
            _changed = true;
            Version++;
            AddDistributedEvent(new HappeningChangedEvent(Id, Version));
        }
    }

    private static class Error
    {
        public static BusinessException NameIsRequired() => new(
            "Event name cannot be white space or empty.");
    }
}
