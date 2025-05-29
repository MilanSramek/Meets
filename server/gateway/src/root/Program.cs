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

    var services = builder.Services;
    services
        .AddHttpClient("Fusion")
        .AddDefaultLogger();

    services
        .AddGraphQLServer();
    services
        .AddFusionGatewayServer(disableDefaultSecurity: true)
        .ConfigureFromFile("Gateway.fgp")
        .ModifyFusionOptions(_ =>
        {
            _.AllowQueryPlan = true;
            _.IncludeDebugInfo = true;
        });

    services
        .AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    var app = builder.Build();

    app.MapGraphQL();
    app.MapReverseProxy();
    
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
