using HotChocolate.Execution;

namespace Meets.Common.Presentation.GraphQL.Subscriptions;

internal sealed class DataWatcher<TDataEvent, TKey, TData> : ISourceStream
{
    private sealed class Reader<T> : IAsyncEnumerable<T>
    {
        private readonly TKey _key;
        private readonly IAsyncEnumerable<TDataEvent> _events;
        private readonly Func<TKey, TDataEvent?, CancellationToken, ValueTask<T>> _dataProvider;

        public Reader(
            TKey key,
            IAsyncEnumerable<TDataEvent> sourceStream,
            Func<TKey, TDataEvent?, CancellationToken, ValueTask<T>> dataProvider)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _events = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async IAsyncEnumerator<T> GetAsyncEnumerator(
            CancellationToken cancellationToken = default)
        {
            var initialData = await _dataProvider(_key, default, cancellationToken);
            yield return initialData;

            await foreach (var dataEvent in _events)
            {
                var data = await _dataProvider(_key, dataEvent, cancellationToken);
                yield return data;
            }
        }
    }

    private readonly TKey _key;
    private readonly ISourceStream<TDataEvent> _sourceStream;
    private readonly Func<TKey, TDataEvent?, CancellationToken, ValueTask<TData>> _dataProvider;

    public DataWatcher(
        TKey key,
        ISourceStream<TDataEvent> sourceStream,
        Func<TKey, TDataEvent?, CancellationToken, ValueTask<TData>> dataProvider)
    {
        _key = key ?? throw new ArgumentNullException(nameof(key));
        _sourceStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
        _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
    }

    public ValueTask DisposeAsync() => _sourceStream.DisposeAsync();

    public IAsyncEnumerable<TData> ReadEventsAsync() => new Reader<TData>(
        _key,
        _sourceStream.ReadEventsAsync(),
        _dataProvider);

    IAsyncEnumerable<object?> ISourceStream.ReadEventsAsync()
    {
        async ValueTask<object?> ProvideData(
            TKey key,
            TDataEvent? dataEvent,
            CancellationToken cancellationToken)
        {
            return await _dataProvider(key, dataEvent, cancellationToken);
        }

        return new Reader<object?>(_key, _sourceStream.ReadEventsAsync(), ProvideData);
    }
}
