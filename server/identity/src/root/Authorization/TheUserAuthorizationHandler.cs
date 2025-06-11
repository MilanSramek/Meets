using HotChocolate.Resolvers;

using Meets.Common.Infrastructure.Identity;
using Meets.Identity.Users;

using Microsoft.AspNetCore.Authorization;

namespace Meets.Identity.Authorization;

internal sealed class TheUserAuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context is { Resource: Guid userId })
        {
            return HandleRequirementsAsync(context, userId);
        }

        if (context is { Resource: IResolverContext resolverContext })
        {
            var userModelId = resolverContext.Parent<UserModel>().Id;
            return HandleRequirementsAsync(context, userModelId);
        }

        return Task.CompletedTask;
    }

    private async Task HandleRequirementsAsync(AuthorizationHandlerContext context, Guid userId)
    {
        foreach (var requirement in context.Requirements.OfType<TheUserRequirement>())
        {
            await HandleRequirementAsync(context, requirement, userId);
        }
    }

    private Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TheUserRequirement requirement,
        Guid userId)
    {
        var currentUserId = context.User.GetUserId();
        if (currentUserId.IsFailure)
        {
            context.Fail(new AuthorizationFailureReason(this, string.Format(
                currentUserId.Error.Message,
                currentUserId.Error.Data)));
            return Task.CompletedTask;
        }

        if (currentUserId.Value == userId)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail(new AuthorizationFailureReason(this,
            "Requested user does not match the current user."));
        return Task.CompletedTask;
    }
}
