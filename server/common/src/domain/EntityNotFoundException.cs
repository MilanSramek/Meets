namespace Meets.Common.Domain;

[Serializable]
public sealed class EntityNotFoundException : BusinessException
{
    private const string EntityTypeKey = "EntityType";
    private const string EntityIdKey = "EntityId";

    public const string CodeValue = "ENTITY_NOT_FOUND";
    public const string MessageValue = "Entity not found";

    public EntityNotFoundException(Type entityType, object entityId)
        : base(CodeValue, MessageValue)
    {
        Data.Add(EntityTypeKey, entityType);
        Data.Add(EntityIdKey, entityId);
    }

    public static EntityNotFoundException Create<TEntity>(object entityId)
        => new(typeof(TEntity), entityId);
}
