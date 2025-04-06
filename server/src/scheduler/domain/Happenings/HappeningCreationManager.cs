using Meets.Scheduler.Polls;

namespace Meets.Scheduler.Happenings;

internal sealed class HappeningCreationManager : IHappeningCreationManager
{
    private readonly IHappeningRepository _eventRepository;
    private readonly IPollRepository _pollRepository;

    public HappeningCreationManager(IHappeningRepository eventRepository, IPollRepository pollRepository)
    {
        _eventRepository = eventRepository
            ?? throw new ArgumentNullException(nameof(eventRepository));
        _pollRepository = pollRepository
            ?? throw new ArgumentNullException(nameof(pollRepository));
    }

    public async Task<Happening> CreateEventAsync<TState>(
        string name,
        Action<TState, Happening> @eventSetter,
        TState state,
        CancellationToken cancellationToken)
    {
        var @event = new Happening(name);
        @eventSetter?.Invoke(state, @event);

        await _eventRepository.InsertAsync(@event, cancellationToken);
        await _pollRepository.InsertAsync(new Poll(@event.Id), cancellationToken);

        return @event;
    }
}
