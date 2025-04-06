namespace Meets.Common.Domain;

public interface IRepository<TEntity, TId> :
    IReadOnlyRepository<TEntity, TId>,
    IInsertRepository<TEntity, TId>,
    IUpdateRepository<TEntity, TId>,
    IDeleteRepository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
}
