using FluentValidation;

using Meets.Identity.Users;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Identity.RegisterEndpoints;

public static class Registrations
{
    public static IServiceCollection AddRegisterEndpoint(
        this IServiceCollection services)
    {
        services.AddSingleton<IValidator<CreateUserInput>,
            CreateUserInputValidator>();

        return services;
    }
}