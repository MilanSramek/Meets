using Meets.Common.Domain;
using Meets.Common.Infrastructure;

using MongoDB.Driver;

namespace Meets.Common.Persistence.MongoDb;

public abstract class MongoRepository<TEntity, TId> :
    ReadOnlyMongoRepository<TEntity, TId>,
    IRepository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    private enum OperationType
    {
        Insert,
        Update,
        Delete
    }

    private static readonly Func<TEntity, object>? s_createdEventFactory;
    private static readonly Func<TEntity, object>? s_changedEventFactory;
    private static readonly Func<TEntity, object>? s_deletedEventFactory;

    private readonly IDomainEventCollectorAccessor _domainEventCollectorAccessor;
    private readonly IIntegrationEventCollectorAccessor _integrationEventCollectorAccessor;

    static MongoRepository()
    {
        var entityType = typeof(TEntity);

        s_createdEventFactory = entityType.IsAssignableTo(typeof(ICreatedIntegrationEventSource))
            ? entity => ((ICreatedIntegrationEventSource)entity).GetCreatedEvent()
            : null;
        s_changedEventFactory = entityType.IsAssignableTo(typeof(IChangedIntegrationEventSource))
            ? entity => ((IChangedIntegrationEventSource)entity).GetChangedEvent()
            : null;
        s_deletedEventFactory = entityType.IsAssignableTo(typeof(IDeletedIntegrationEventSource))
            ? entity => ((IDeletedIntegrationEventSource)entity).GetDeletedEvent()
            : null;
    }

    public MongoRepository(
        IMongoDatabase database,
        IDomainEventCollectorAccessor eventCollectorAccessor,
        IIntegrationEventCollectorAccessor integrationEventCollectorAccessor)
        : base(database)
    {
        _domainEventCollectorAccessor = eventCollectorAccessor
            ?? throw new ArgumentNullException(nameof(eventCollectorAccessor));
        _integrationEventCollectorAccessor = integrationEventCollectorAccessor
            ?? throw new ArgumentNullException(nameof(integrationEventCollectorAccessor));
    }

    public ValueTask DeleteAsync(CancellationToken cancellationToken,
        params ReadOnlySpan<TEntity> entities)
    {
        return entities switch
        {
            { IsEmpty: true } => ValueTask.CompletedTask,
            [var entity] => new(DeleteOneAsync(entity.Id, cancellationToken)),
            _ => new(DeleteManyAsync([.. entities], cancellationToken))
        };
    }

    public ValueTask InsertAsync(CancellationToken cancellationToken,
        params ReadOnlySpan<TEntity> entities)
    {
        return entities switch
        {
            { IsEmpty: true } => ValueTask.CompletedTask,
            [var entity] => InsertOneAsync(entity, cancellationToken),
            _ => InsertManyAsync([.. entities], cancellationToken)
        };
    }

    public ValueTask UpdateAsync(CancellationToken cancellationToken,
        params ReadOnlySpan<TEntity> entities)
    {
        return entities switch
        {
            { IsEmpty: true } => ValueTask.CompletedTask,
            [var entity] => new(UpdateOneAsync(entity, cancellationToken)),
            _ => UpdateManyAsync([.. entities], cancellationToken)
        };
    }

    private async Task DeleteOneAsync(TId id, CancellationToken cancellationToken)
    {
        var result = await Collection.DeleteOneAsync(_ => _.Id.Equals(id),
            cancellationToken);
        if (result.DeletedCount == 1)
        {
            return;
        }

        throw new Exception($"Failed to delete entity with id '{id}'.");
    }

    private async Task DeleteManyAsync(TEntity[] entities, CancellationToken cancellationToken)
    {
        var ids = entities.Select(e => e.Id);
        var result = await Collection.DeleteOneAsync(_ => ids.Contains(_.Id),
            cancellationToken);

        if (result.DeletedCount == entities.Length)
        {
            ReportIntegrationAutoEvent(OperationType.Delete, entities);
            return;
        }

        throw new Exception($"Failed to delete entities.");
    }

    private async ValueTask InsertOneAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ReportDomainEvents(entity);
        await Collection.InsertOneAsync(entity, default, cancellationToken);
        ReportIntegrationAutoEvent(OperationType.Insert, entity);
        ReportIntegrationEvents(entity);
    }

    private async ValueTask InsertManyAsync(TEntity[] entities, CancellationToken cancellationToken)
    {
        ReportDomainEvents(entities);
        await Collection.InsertManyAsync(entities, default, cancellationToken);
        ReportIntegrationAutoEvent(OperationType.Insert, entities);
        ReportIntegrationEvents(entities);
    }

    private async Task UpdateOneAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ReportDomainEvents(entity);

        var result = await Collection.ReplaceOneAsync(
            _ => _.Id.Equals(entity.Id),
            entity,
            default(ReplaceOptions),
            cancellationToken);
        if (result.ModifiedCount == 1)
        {
            ReportIntegrationAutoEvent(OperationType.Update, entity);
            ReportIntegrationEvents(entity);
            return;
        }

        throw new Exception($"Failed to update entity with id '{entity.Id}'.");
    }

    private async ValueTask UpdateManyAsync(TEntity[] entities,
        CancellationToken cancellationToken)
    {
        ReportDomainEvents(entities);

        var bulkUpdate = new List<WriteModel<TEntity>>(entities.Length);
        foreach (var entity in entities)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            var update = Builders<TEntity>.Update.Set(e => e, entity);

            var updateOneModel = new ReplaceOneModel<TEntity>(filter, entity)
            {
                IsUpsert = false
            };

            bulkUpdate.Add(updateOneModel);
        }

        var result = await Collection.BulkWriteAsync(
            bulkUpdate,
            default,
            cancellationToken);
        if (result.ModifiedCount + result.InsertedCount == bulkUpdate.Count)
        {
            ReportIntegrationAutoEvent(OperationType.Update, entities);
            ReportIntegrationEvents(entities);
            return;
        }

        throw new Exception("Failed to update entities.");
    }

    private void ReportDomainEvents(params ReadOnlySpan<TEntity> entities)
    {
        var eventCollector = _domainEventCollectorAccessor.EventCollector;
        if (eventCollector is null)
        {
            return;
        }

        foreach (var entity in entities)
        {
            eventCollector.AddEvents(entity.DomainEvents);
        }
    }

    private void ReportIntegrationEvents(params ReadOnlySpan<TEntity> entities)
    {
        var eventCollector = _integrationEventCollectorAccessor.EventCollector;
        if (eventCollector is null)
        {
            return;
        }

        foreach (var entity in entities)
        {
            eventCollector.AddEvents(entity.IntegrationEvents);
        }
    }

    private void ReportIntegrationAutoEvent(OperationType operation,
        params ReadOnlySpan<TEntity> entities)
    {
        var eventCollector = _integrationEventCollectorAccessor.EventCollector;
        if (eventCollector is null)
        {
            return;
        }

        var eventFactory = operation switch
        {
            OperationType.Insert => s_createdEventFactory,
            OperationType.Update => s_changedEventFactory,
            OperationType.Delete => s_deletedEventFactory,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };
        if (eventFactory is null)
        {
            return;
        }

        foreach (var entity in entities)
        {
            object @event = eventFactory.Invoke(entity);
            eventCollector.AddEvent(@event);
        }
    }
}
