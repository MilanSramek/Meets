using Meets.Common.Infrastructure;
using Meets.Common.Persistence.MongoDb;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var authSection = configuration.GetRequiredSection("authentication");
            options.Authority = authSection.GetValue<string>("authority");
            options.Audience = authSection.GetValue<string>("audience");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = false,
                ValidIssuer = authSection.GetValue<string>("authority"),
            };

            // Optional: disable HTTPS requirement during local development
            options.RequireHttpsMetadata = false;
        });
    services.AddAuthorization();

    var app = builder.Build();

    app.Services.InitializeGraphQLPresentation();

    if (app.Environment.IsDevelopment())
    {
        app.UseCors("AllowAllOrigins");
    }

    app.UseSerilogRequestLogging();
    app.UseForwardedHeaders();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

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
