namespace Meets.Scheduler.Activities;

public interface IActivityWatcherProvider
{
    public ValueTask<IAsyncEnumerable<ActivityModel>> GetWatcherAsync(Guid eventId,
        CancellationToken cancellationToken);
}
