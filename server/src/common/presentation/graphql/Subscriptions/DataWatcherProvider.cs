using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace Meets.Common.Presentation.GraphQL.Subscriptions;

internal sealed class DataWatcherProvider<TData, TKey, TDataEvent> : IDataWatcherProvider<TData, TKey>
{
    private readonly Func<TKey, string> _topicPicker;
    private readonly Func<TKey, TDataEvent?, CancellationToken, ValueTask<TData>> _dataProvider;
    private readonly ITopicEventReceiver _receiver;

    public DataWatcherProvider(
        Func<TKey, string> topicPicker,
        Func<TKey, TDataEvent?, CancellationToken, ValueTask<TData>> dataProvider,
        ITopicEventReceiver receiver)
    {
        _topicPicker = topicPicker ?? throw new ArgumentNullException(nameof(topicPicker));
        _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
    }

    public async ValueTask<ISourceStream> GetWatcherAsync(
        TKey key,
        CancellationToken cancellationToken)
    {
        string topic = _topicPicker(key);
        var sourceStream = await _receiver.SubscribeAsync<TDataEvent>(topic,
            cancellationToken);
        return new DataWatcher<TDataEvent, TKey, TData>(key, sourceStream, _dataProvider);
    }
}
