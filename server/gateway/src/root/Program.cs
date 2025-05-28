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
        .AddHttpClient("Try")
        .AddDefaultLogger();

    builder.Services
        .AddFusionGatewayServer(disableDefaultSecurity: true)
        .ConfigureFromFile("Gateway.fgp")
        // Note: AllowQueryPlan is enabled for demonstration purposes. Disable in production environments.
        .ModifyFusionOptions(_ =>
        {
            _.AllowQueryPlan = true;
            _.IncludeDebugInfo = true;
        });

    builder.Services.AddSingleton(new RequestCostOptions(100, 100, true, 10));

    builder.WebHost.UseUrls("http://*:80");
    var app = builder.Build();

    app.MapGraphQL();
    app.MapGet("/try", async (HttpContext context) =>
    {
        var client = context.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient("Try");
        var response = await client.GetAsync("http://localhost:8081/graphql?query={__typename}");
        response.EnsureSuccessStatusCode();
        await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
    });

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
