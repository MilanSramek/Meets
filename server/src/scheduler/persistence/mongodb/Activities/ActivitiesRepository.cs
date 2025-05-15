using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;

using MongoDB.Driver;

namespace Meets.Scheduler.Activities;

internal sealed class ActivitiesRepository
(
    IMongoDatabase database,
    IDomainEventCollectorAccessor eventCollectorAccessor,
    IIntegrationEventCollectorAccessor integrationEventCollectorAccessor
) :
    MongoRepository<Activity, Guid>(database, eventCollectorAccessor, integrationEventCollectorAccessor),
    IActivityRepository
{
}
