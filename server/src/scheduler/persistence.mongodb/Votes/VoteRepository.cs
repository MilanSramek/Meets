using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;

using MongoDB.Driver;

namespace Meets.Scheduler.Votes;

internal sealed class VoteRepository
(
    IMongoDatabase database,
    IDomainEventCollectorAccessor eventCollectorAccessor,
    IIntegrationEventCollectorAccessor integrationEventCollectorAccessor
) :
    MongoRepository<Vote, Guid>(database, eventCollectorAccessor, integrationEventCollectorAccessor),
    IVoteRepository
{
}
