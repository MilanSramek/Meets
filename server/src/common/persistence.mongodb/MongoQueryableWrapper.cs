using MongoDB.Driver.Linq;

using System.Collections;
using System.Linq.Expressions;

namespace Meets.Common.Persistence.MongoDb;

internal sealed class MongoQueryableWrapper : IQueryable
{
    private readonly IQueryable _innerQueryable;

    public MongoQueryableWrapper(
        IQueryable innerQueryable,
        MongoQueryProviderWrapper queryProvider)
    {
        _innerQueryable = innerQueryable
            ?? throw new ArgumentNullException(nameof(innerQueryable));
        Provider = queryProvider
            ?? throw new ArgumentNullException(nameof(queryProvider));
    }

    public Type ElementType => _innerQueryable.ElementType;
    public Expression Expression => _innerQueryable.Expression;
    public IQueryProvider Provider { get; }

    IEnumerator IEnumerable.GetEnumerator() => _innerQueryable.GetEnumerator();
}


internal sealed class MongoQueryableWrapper<T> : IQueryable<T>, IAsyncEnumerable<T>
{
    private readonly IQueryable<T> _innerQueryable;

    public MongoQueryableWrapper(
        IQueryable<T> innerQueryable,
        MongoQueryProviderWrapper queryProvider)
    {
        _innerQueryable = innerQueryable
            ?? throw new ArgumentNullException(nameof(innerQueryable));
        Provider = queryProvider
            ?? throw new ArgumentNullException(nameof(queryProvider));
    }

    public Type ElementType => _innerQueryable.ElementType;
    public Expression Expression => _innerQueryable.Expression;
    public IQueryProvider Provider { get; }

    public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        var cursor = await _innerQueryable.ToCursorAsync(cancellationToken);
        while (await cursor.MoveNextAsync(cancellationToken))
        {
            foreach (var item in cursor.Current)
            {
                yield return item;
            }
        }
    }

    public IEnumerator<T> GetEnumerator() => _innerQueryable.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
