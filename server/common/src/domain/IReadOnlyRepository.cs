namespace Meets.Common.Domain;

public interface IReadOnlyRepository<TEntity, TId> : IQueryable<TEntity>, IAsyncEnumerable<TEntity>
    where TEntity : IAggregateRoot<TId>
    where TId : notnull
{
}
