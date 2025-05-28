using HotChocolate.CostAnalysis;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddHttpClient("Fusion")
        .AddDefaultLogger();

    builder.Services
        .AddGraphQLServer();
    builder.Services
        .AddFusionGatewayServer(disableDefaultSecurity: true)
        .ConfigureFromFile("gateway.fgp")
        .ModifyFusionOptions(_ =>
        {
            _.AllowQueryPlan = true;
            _.IncludeDebugInfo = true;
        });

    builder.Services.AddSingleton(new RequestCostOptions(100, 100, true, 10));

    builder.WebHost.UseUrls("http://*:80");
    var app = builder.Build();

    app.MapGraphQL();
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
