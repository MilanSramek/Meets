namespace Meets.Scheduler.Happenings;

public interface IHappeningCreationManager
{
    public Task<Happening> CreateEventAsync<TState>(
        string name,
        Action<TState, Happening> eventSetter,
        TState state,
        CancellationToken cancellationToken);
}
