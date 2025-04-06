using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;

using MongoDB.Driver;

namespace Meets.Scheduler.Happenings;

internal sealed class HappeningsRepository
(
    IMongoDatabase database,
    IDomainEventCollectorAccessor eventCollectorAccessor,
    IIntegrationEventCollectorAccessor integrationEventCollectorAccessor
) :
    MongoRepository<Happening, Guid>(database, eventCollectorAccessor, integrationEventCollectorAccessor),
    IHappeningRepository
{
}
