using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Meets.Identity.Users;
using FluentValidation;

namespace Meets.Identity.RegisterEndpoints;

public static class RegisterEndpoint
{
    public static void MapRegisterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(EndpointPath.Register, async (
            [FromBody] CreateUserInput request,
            [FromServices] IValidator<CreateUserInput> validator,
            [FromServices] IAccountService accountService,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request,
                cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new ValidationProblemDetails
                {
                    Title = "Validation failed",
                    Errors = validationResult.Errors.ToDictionary(
                        error => error.PropertyName,
                        error => new[] { error.ErrorMessage })
                });
            }

            var result = await accountService.CreateUserAsync(request,
                cancellationToken);

            return result switch
            {
                { Succeeded: true } => Results.Ok(),
                { IsConflicted: true } => Results.Conflict("User already exists."),
                { Errors: { } errors } => Results.BadRequest(new ValidationProblemDetails
                {
                    Title = "User creation failed",
                    Errors = errors.ToDictionary(
                            error => error.Code,
                            error => new[] { error.Description })
                }),
                _ => Results.BadRequest("User creation failed.")
            };
        })
        .WithSummary("Register a new user")
        .WithTags("Registration")
        .Produces(StatusCodes.Status200OK)
        .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);
    }
}
