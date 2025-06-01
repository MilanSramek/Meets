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
        .AddHttpMessageHandler<AuthForwardingHandler>() // ToDo: Rewrite to use RemoteContext
        .AddDefaultLogger();
    services
        .AddSingleton<AuthForwardingHandler>()
        .AddHttpContextAccessor();

    services
        .AddGraphQLServer();
    services
        .AddFusionGatewayServer(disableDefaultSecurity: true) // ToDo: Remove in production
        .ConfigureFromFile("Gateway.fgp")
        .ModifyFusionOptions(_ =>
        {
            _.AllowQueryPlan = true;
            _.IncludeDebugInfo = true; // ToDo: Remove in production
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
