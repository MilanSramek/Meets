namespace Meets.Scheduler.Activities;

public sealed class Activity : AggregateRoot<Guid>, IWithVersion
{
    private bool _changed = false;

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid? OwnerId { get; private set; }
    public int Version { get; private set; }

    private Activity()
    {
    }

    internal Activity(string name)
    {
        SetName(name);
    }

    internal Activity(Guid id, string name) : this(name)
    {
        Id = id;
    }

    public Activity SetName(string name)
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
        return this;
    }

    public Activity SetDescription(string? description)
    {
        if (Description != description)
        {
            Description = description;
            SetChanged();
        }
        return this;
    }

    public Activity SetOwner(Guid? ownerId)
    {
        if (OwnerId != ownerId)
        {
            OwnerId = ownerId;
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
            AddDistributedEvent(new ActivityChangedEvent(Id, Version));
        }
    }

    private static class Error
    {
        public static BusinessException NameIsRequired() => new(
            "ACTIVITY_NAME_REQUIRED",
            "Activity name cannot be white space or empty.");
    }
}
