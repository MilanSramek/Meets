var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Fusion");

builder.Services
    .AddFusionGatewayServer()
    .ConfigureFromFile("gateway.fgp")
    // Note: AllowQueryPlan is enabled for demonstration purposes. Disable in production environments.
    .ModifyFusionOptions(x => x.AllowQueryPlan = true);

builder.WebHost.UseUrls("http://*:80");
var app = builder.Build();

app.MapGraphQL();

app.Run();