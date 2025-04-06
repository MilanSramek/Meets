using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;
using Meets.Scheduler;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services
    .AddLogging(log =>
    {
        log.AddConsole();
    });

services
    .AddGraphQLPresentation()
    .AddMongoDbPersistence()
    .AddInfrastructure()
    .AddApplication()
    .AddDomain();

var configuration = builder.Configuration;
services
    .AddOptions<MongoClientDbOptions>()
    .Bind(configuration.GetSection(MongoClientDbOptions.SectionName))
    .ValidateDataAnnotations();

var app = builder.Build();
app.Services.InitializeGraphQLPresentation();

app.MapGraphQL("/graphql");
await app.RunAsync();
