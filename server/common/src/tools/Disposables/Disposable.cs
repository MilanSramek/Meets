namespace Meets.Common.Tools.Disposables;

public sealed class Disposable
{
    public static CombinedAsyncDisposable Combine(params IAsyncDisposable[] disposables) =>
        new(disposables);
}
