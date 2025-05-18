using MongoDB.Driver.Linq;

using System.Linq.Expressions;

namespace Meets.Common.Persistence.MongoDb;

internal sealed class MongoQueryProviderWrapper : IAsyncQueryProvider
{
    public IMongoQueryProvider InnerQueryProvider { get; }

    public MongoQueryProviderWrapper(IMongoQueryProvider innerQueryProvider)
    {
        InnerQueryProvider = innerQueryProvider
            ?? throw new ArgumentNullException(nameof(innerQueryProvider));
    }

    public IQueryable CreateQuery(Expression expression)
    {
        var innerQueryable = InnerQueryProvider.CreateQuery(expression);
        return innerQueryable.Provider == InnerQueryProvider
            ? new MongoQueryableWrapper(innerQueryable, this)
            : new MongoQueryableWrapper(
                innerQueryable,
                new MongoQueryProviderWrapper((IMongoQueryProvider)innerQueryable.Provider));
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        var innerQueryable = InnerQueryProvider.CreateQuery<TElement>(expression);
        return innerQueryable.Provider == InnerQueryProvider
            ? new MongoQueryableWrapper<TElement>(innerQueryable, this)
            : new MongoQueryableWrapper<TElement>(
                innerQueryable,
                new MongoQueryProviderWrapper((IMongoQueryProvider)innerQueryable.Provider));
    }

    public object? Execute(Expression expression) => InnerQueryProvider
        .Execute(expression);

    public TResult Execute<TResult>(Expression expression) => InnerQueryProvider
        .Execute<TResult>(expression);

    public Task<TResult> ExecuteAsync<TResult>(Expression expression,
        CancellationToken cancellationToken)
    {
        return InnerQueryProvider.ExecuteAsync<TResult>(expression, cancellationToken);
    }
}
