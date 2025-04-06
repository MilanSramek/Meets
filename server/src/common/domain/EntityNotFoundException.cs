namespace Meets.Common.Domain;

[Serializable]
public sealed class EntityNotFoundException : BusinessException
{
    private const string EntityTypeKey = "EntityType";
    private const string EntityIdKey = "EntityId";

    public EntityNotFoundException(Type entityType, object entityId)
        : base("Entity not found.")
    {
        Data.Add(EntityTypeKey, entityType);
        Data.Add(EntityIdKey, entityId);
    }

    public static EntityNotFoundException Create<TEntity>(object entityId)
        => new EntityNotFoundException(typeof(TEntity), entityId);
}
