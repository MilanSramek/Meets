using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;

namespace Meets.Scheduler.Happenings;

internal sealed class HappeningWatcherEnumerator : IAsyncEnumerator<HappeningModel>
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
    private readonly HappeningWatcherEnumeratorUnsubscriber _unsubscriber;
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenRegistration _cancellationTokenRegistration;
    private HappeningModel _event;
    private HappeningModel? _current;

    public HappeningWatcherEnumerator(
        HappeningModel initialValue,
        HappeningWatcherEnumeratorUnsubscriber unsubscriber,
        CancellationToken cancellationToken)
    {
        _event = initialValue;

        _unsubscriber = unsubscriber;
        _cancellationToken = cancellationToken;

        _cancellationTokenRegistration = cancellationToken.Register(Cancel);
    }

    public HappeningModel Current => _current
        ?? throw new InvalidOperationException("Enumeration has not started.");

    public async ValueTask<bool> MoveNextAsync()
    {
        _cancellationToken.ThrowIfCancellationRequested();

        var @event = _event;
        if (_current != @event)
        {
            _current = @event;
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

    public void Publish(HappeningModel @event)
    {
        if (_event != @event)
        {
            _event = @event;
            _semaphore.Signalize();
        }
    }

    private void Cancel() => _semaphore.Cancel();
}
