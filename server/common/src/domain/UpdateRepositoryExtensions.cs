namespace Meets.Common.Domain;

public static class UpdateRepositoryExtensions
{
    public static ValueTask UpdateAsync<TEntity, TId>(
        this IUpdateRepository<TEntity, TId> repository,
        TEntity entity,
        CancellationToken cancellationToken)
        where TEntity : IAggregateRoot<TId>
        where TId : notnull
    {
        return repository.UpdateAsync(cancellationToken, entity);
    }
}
