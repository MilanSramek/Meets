
using HotChocolate.Execution;

namespace Meets.Common.Presentation.Graphql.Subscriptions;

public interface IDataWatcherProvider<TData, TKey>
{
    public ValueTask<ISourceStream> GetWatcherAsync(TKey key,
        CancellationToken cancellationToken);
}
