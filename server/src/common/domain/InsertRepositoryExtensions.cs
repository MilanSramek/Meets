namespace Meets.Common.Domain;

public static class InsertRepositoryExtensions
{
    public static ValueTask InsertAsync<TEntity, TId>(
        this IInsertRepository<TEntity, TId> repository,
        TEntity entity,
        CancellationToken cancellationToken)
        where TEntity : AggregateRoot<TId>
        where TId : notnull
    {
        return repository.InsertAsync(cancellationToken, entity);
    }
}
