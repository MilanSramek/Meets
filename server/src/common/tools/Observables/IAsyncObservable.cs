namespace Meets.Common.Tools.Observables;

public interface IAsyncObservable<out T>
{
    public IAsyncDisposable SubscribeAsync(IAsyncObserver<T> observer);
}
