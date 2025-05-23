using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;

using MongoDB.Driver;

namespace Meets.Identity.ApplicationUsers;

internal sealed class ApplicationUserRepository
(
    IMongoDatabase database,
    IDomainEventCollectorAccessor eventCollectorAccessor,
    IIntegrationEventCollectorAccessor integrationEventCollectorAccessor
) :
    MongoRepository<ApplicationUser, Guid>(
        database,
        eventCollectorAccessor,
        integrationEventCollectorAccessor),
    IApplicationUserRepository
{
}