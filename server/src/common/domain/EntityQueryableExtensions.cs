namespace Meets.Common.Domain;

public static class EntityQueryableExtensions
{
    public static async Task<TEntity> GetAsync<TEntity, TId>(
        this IQueryable<TEntity> queryable,
        TId id,
        CancellationToken cancellationToken)
        where TId : notnull
        where TEntity : IEntity<TId>
    {
        var entity = await queryable
            .FirstOrDefaultAsync(_ => _.Id.Equals(id), cancellationToken);
        return entity ?? throw EntityNotFoundException.Create<TEntity>(id);
    }
}
