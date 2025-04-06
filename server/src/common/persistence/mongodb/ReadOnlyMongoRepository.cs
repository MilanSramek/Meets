using Meets.Common.Domain;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

using System.Collections;
using System.Linq.Expressions;

namespace Meets.Common.Persistence.MongoDb;

public abstract class ReadOnlyMongoRepository<TEntity, TId> : IReadOnlyRepository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    private MongoQueryableWrapper<TEntity>? _queryable;
    private MongoQueryProviderWrapper? _provider;

    public ReadOnlyMongoRepository(IMongoDatabase database)
    {
        ArgumentNullException.ThrowIfNull(database);
        Collection = database.GetCollection<TEntity>();
    }

    public Type ElementType => Queryable.ElementType;
    public Expression Expression => Queryable.Expression;
    public IQueryProvider Provider
    {
        get
        {
            if (_provider is null)
            {
                InitializeQueryable();
            }
            return _provider!;
        }
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(
        CancellationToken cancellationToken = default)
    {
        return ((IAsyncEnumerable<TEntity>)Queryable).GetAsyncEnumerator(cancellationToken);
    }

    public IEnumerator<TEntity> GetEnumerator() => Queryable.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected IMongoCollection<TEntity> Collection { get; }
    protected IQueryable<TEntity> Queryable
    {
        get
        {
            if (_queryable is null)
            {
                InitializeQueryable();
            }
            return _queryable!;
        }
    }

    private void InitializeQueryable()
    {
        var innerQueryable = Collection.AsQueryable();

        _provider = new MongoQueryProviderWrapper((IMongoQueryProvider)innerQueryable.Provider);
        _queryable = new MongoQueryableWrapper<TEntity>(innerQueryable, _provider);
    }
}
