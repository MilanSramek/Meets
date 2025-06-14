namespace Meets.Common.Domain;

public interface IInsertRepository<TEntity, TId> : IReadOnlyRepository<TEntity, TId>
    where TEntity : IAggregateRoot<TId>
    where TId : notnull
{
    public ValueTask InsertAsync(CancellationToken cancellationToken,
        params ReadOnlySpan<TEntity> entities);
}
