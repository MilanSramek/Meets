using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;

using MongoDB.Driver;

namespace Meets.Identity.Users;

internal sealed class UserRepository
(
    IMongoDatabase database,
    IDomainEventCollectorAccessor eventCollectorAccessor,
    IIntegrationEventCollectorAccessor integrationEventCollectorAccessor
) :
    MongoRepository<User, Guid>(
        database,
        eventCollectorAccessor,
        integrationEventCollectorAccessor),
    IUserRepository
{
}