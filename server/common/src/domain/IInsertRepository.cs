namespace Meets.Common.Domain;

public interface IInsertRepository<TEntity, TId> : IReadOnlyRepository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    public ValueTask InsertAsync(CancellationToken cancellationToken,
        params ReadOnlySpan<TEntity> entities);
}
