namespace Meets.Common.Domain;

public static class UpdateRepositoryExtensions
{
    public static ValueTask UpdateAsync<TEntity, TId>(
        this IUpdateRepository<TEntity, TId> repository,
        TEntity entity,
        CancellationToken cancellationToken)
        where TEntity : AggregateRoot<TId>
        where TId : notnull
    {
        return repository.UpdateAsync(cancellationToken, entity);
    }
}
