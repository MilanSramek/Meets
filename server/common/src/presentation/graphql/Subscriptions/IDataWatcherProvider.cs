
using HotChocolate.Execution;

namespace Meets.Common.Presentation.GraphQL.Subscriptions;

public interface IDataWatcherProvider<TData, TKey>
{
    public ValueTask<ISourceStream> GetWatcherAsync(TKey key,
        CancellationToken cancellationToken);
}
