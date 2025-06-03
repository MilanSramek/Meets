using OpenIddict.Abstractions;
using OpenIddict.Server;

using static OpenIddict.Server.OpenIddictServerEvents;

namespace Meets.Identity.Core;

internal sealed class SignInHandler : IOpenIddictServerHandler<ProcessSignInContext>
{
    public ValueTask HandleAsync(ProcessSignInContext context)
    {
        if (context.Principal != null && context.AccessTokenPrincipal != null)
        {
            context.AccessTokenPrincipal.SetAudiences("meets");
        }

        return ValueTask.CompletedTask;
    }
}