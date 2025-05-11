using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;
using Meets.Scheduler;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting application");

    var builder = WebApplication.CreateBuilder(args);

    var services = builder.Services;
    services
        .AddSerilog((services, config) => config
            .Enrich.FromLogContext()
            .WriteTo.Console());

    if (builder.Environment.IsDevelopment())
    {
        services.AddCors(options => options.AddPolicy("AllowAllOrigins", _ => _
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));
    }

    services
        .AddGraphQLPresentation()
        .AddMongoDbPersistence()
        .AddInfrastructure()
        .AddApplication()
        .AddDomain();

    var configuration = builder.Configuration;
    configuration
        .AddKeyPerFile("/run/secrets", optional: true); // docker secret

    services
        .AddOptions<MongoClientDbOptions>()
        .Bind(configuration.GetSection("db"))
        .ValidateDataAnnotations();

    builder.WebHost.UseUrls("http://*:80");
    var app = builder.Build();

    app.Services.InitializeGraphQLPresentation();

    app.UseSerilogRequestLogging();
    if (app.Environment.IsDevelopment())
    {
        app.UseCors("AllowAllOrigins");
    }
    app.MapGraphQL("/graphql");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
