
namespace Meets.Common.Tools.Disposables;

public sealed class CombinedAsyncDisposable : IAsyncDisposable
{
    private readonly IAsyncDisposable[] _disposables;

    public CombinedAsyncDisposable(params ReadOnlySpan<IAsyncDisposable> disposables)
    {
        _disposables = [.. disposables];
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var disposable in _disposables)
        {
            await disposable.DisposeAsync();
        }
    }
}
