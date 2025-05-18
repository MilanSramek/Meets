using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;

namespace Meets.Scheduler.Activities;

internal sealed class ActivityWatcherEnumerator : IAsyncEnumerator<ActivityModel>
{
    private sealed class Semaphore : IValueTaskSource
    {
        private ManualResetValueTaskSourceCore<object?> _core = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetResult(short token) => _core.GetResult(token);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueTaskSourceStatus GetStatus(short token) => _core.GetStatus(token);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(
            Action<object?> continuation,
            object? state,
            short token,
            ValueTaskSourceOnCompletedFlags flags)
        {
            _core.OnCompleted(continuation, state, token, flags); ;
        }

        public short Version => _core.Version;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Signalize() => _core.SetResult(null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset() => _core.Reset();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Cancel() => _core.SetException(new OperationCanceledException());
    }

    private readonly Semaphore _semaphore = new();
    private readonly ActivityWatcherEnumeratorUnsubscriber _unsubscriber;
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenRegistration _cancellationTokenRegistration;
    private ActivityModel _activity;
    private ActivityModel? _current;

    public ActivityWatcherEnumerator(
        ActivityModel initialValue,
        ActivityWatcherEnumeratorUnsubscriber unsubscriber,
        CancellationToken cancellationToken)
    {
        _activity = initialValue;

        _unsubscriber = unsubscriber;
        _cancellationToken = cancellationToken;

        _cancellationTokenRegistration = cancellationToken.Register(Cancel);
    }

    public ActivityModel Current => _current
        ?? throw new InvalidOperationException("Enumeration has not started.");

    public async ValueTask<bool> MoveNextAsync()
    {
        _cancellationToken.ThrowIfCancellationRequested();

        var activity = _activity;
        if (_current != activity)
        {
            _current = activity;
            return true;
        }

        await new ValueTask(_semaphore, _semaphore.Version);
        _semaphore.Reset();

        return true;
    }

    public async ValueTask DisposeAsync()
    {
        await _unsubscriber.UnsubscribeAsync(this);
        await _cancellationTokenRegistration.DisposeAsync();
    }

    public void Publish(ActivityModel activity)
    {
        if (_activity != activity)
        {
            _activity = activity;
            _semaphore.Signalize();
        }
    }

    private void Cancel() => _semaphore.Cancel();
}
