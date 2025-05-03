using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;
using Meets.Scheduler;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.WebHost.UseUrls("http://*:80");

var services = builder.Services;

if (builder.Environment.IsDevelopment())
{
    services.AddCors(options => options.AddPolicy("AllowAllOrigins", _ => _
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()));

    builder.Logging.AddDebug();
}

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
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
}
app.MapGraphQL("/graphql");
await app.RunAsync();
