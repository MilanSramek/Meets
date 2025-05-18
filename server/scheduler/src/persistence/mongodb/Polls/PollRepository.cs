using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;

using MongoDB.Driver;

namespace Meets.Scheduler.Polls;

internal sealed class PollRepository
(
    IMongoDatabase database,
    IDomainEventCollectorAccessor eventCollectorAccessor,
    IIntegrationEventCollectorAccessor integrationEventCollectorAccessor
) :
    MongoRepository<Poll, Guid>(database, eventCollectorAccessor, integrationEventCollectorAccessor),
    IPollRepository
{
}
