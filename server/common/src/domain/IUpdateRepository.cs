namespace Meets.Common.Domain;

public interface IUpdateRepository<TEntity, TId> : IReadOnlyRepository<TEntity, TId>
    where TEntity : IAggregateRoot<TId>
    where TId : notnull
{
    public ValueTask UpdateAsync(CancellationToken cancellationToken,
        params ReadOnlySpan<TEntity> entities);
}
