namespace Meets.Common.Domain;

public interface IReadOnlyRepository<TEntity, TId> : IQueryable<TEntity>, IAsyncEnumerable<TEntity>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
}
