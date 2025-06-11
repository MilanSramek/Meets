// using Meets.Common.Application.Identity;

// using Microsoft.AspNetCore.Authorization;

// namespace Meets.Identity.Authorization;

// internal sealed class TheUserHandler :
//     AuthorizationHandler<TheUserRequirement, Guid>
// {
//     private readonly IIdentityContext _identityContext;

//     public TheUserHandler(IIdentityContext identityContext)
//     {
//         _identityContext = identityContext;
//     }

//     protected override Task HandleRequirementAsync(
//         AuthorizationHandlerContext context,
//         TheUserRequirement requirement,
//         Guid userId)
//     {
//         if (_identityContext.UserId == userId)
//         {
//             context.Succeed(requirement);
//             return Task.CompletedTask;
//         }

//         context.Fail();
//         return Task.CompletedTask;
//     }
// }