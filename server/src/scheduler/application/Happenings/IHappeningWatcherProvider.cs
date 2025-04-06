namespace Meets.Scheduler.Happenings;

public interface IHappeningWatcherProvider
{
    public ValueTask<IAsyncEnumerable<HappeningModel>> GetWatcherAsync(Guid eventId,
        CancellationToken cancellationToken);
}
