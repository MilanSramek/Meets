var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddGraphQLPresentation();

var app = builder.Build();

await app.RunWithGraphQLCommandsAsync(args);