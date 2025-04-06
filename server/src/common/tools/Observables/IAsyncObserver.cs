namespace Meets.Common.Tools.Observables;

public interface IAsyncObserver<in T>
{
    public ValueTask OnNextAsync(T value, CancellationToken cancellationToken);
    public ValueTask OnErrorAsync(Exception error, CancellationToken cancellationToken);
    public ValueTask OnCompletedAsync(CancellationToken cancellationToken);
}
