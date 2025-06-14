namespace Meets.Common.Domain;

public interface IDeleteRepository<TEntity, TId> : IReadOnlyRepository<TEntity, TId>
    where TEntity : IAggregateRoot<TId>
    where TId : notnull
{
    public ValueTask DeleteAsync(CancellationToken cancellationToken,
        params ReadOnlySpan<TEntity> entities);
}
