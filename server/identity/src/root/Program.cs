using Meets.Common.Infrastructure;
using Meets.Common.Infrastructure.Hosting;
using Meets.Common.Infrastructure.Identity;
using Meets.Common.Persistence.MongoDb;
using Meets.Identity.Authorization;

using Microsoft.AspNetCore.Authorization;

using OpenIddict.Validation.AspNetCore;

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

    if (builder.Environment.IsEnvironment("Development.StandAlone"))
    {
        builder.Configuration.AddUserSecrets<Program>();
    }

    if (builder.Environment.IsOneOfDevelopment())
    {
        services.AddCors(options => options.AddPolicy("AllowAllOrigins", _ => _
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));
    }

    services
        .AddRestPresentation()
        .AddGraphQLPresentation()
        .AddApplication()
        .AddPersistence()
        .AddInfrastructure();

    var configuration = builder.Configuration;
    configuration
        .AddKeyPerFile("/run/secrets", optional: true); // docker secret

    services.AddOptions<MongoClientDbOptions>()
        .Bind(configuration.GetSection("db"))
        .ValidateDataAnnotations();

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    });
    services.AddOpenIddict()
        .AddValidation(options =>
        {
            options.UseLocalServer();
            options.UseAspNetCore();
        });
    services.AddAuthorization(options =>
    {
        options.AddPolicy("the-user", policy => policy.AddRequirements(new TheUserRequirement()));
    });
    services.AddSingleton<IAuthorizationHandler, TheUserAuthorizationHandler>();

    services.AddAntiforgery();

    var app = builder.Build();

    if (app.Environment.IsOneOfDevelopment())
    {
        app.UseCors("AllowAllOrigins");
        app.UseDeveloperExceptionPage();
        app.MapOpenApi("/api/openapi/{documentName}.json");
    }
    app.UseSerilogRequestLogging();
    app.UseForwardedHeaders();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseAntiforgery();
    app.UseIdentityContext();
    app.MapGraphQL();
    app.MapAuthorityEndpoints();

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
