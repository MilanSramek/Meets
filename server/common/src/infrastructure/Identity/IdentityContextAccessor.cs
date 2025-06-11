using Meets.Common.Application.Identity;

using Microsoft.AspNetCore.Http;

namespace Meets.Common.Infrastructure.Identity;

internal sealed class IdentityContextAccessor : IIdentityContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IIdentityContext? Context
    {
        get => _httpContextAccessor.HttpContext?.GetIdentityContext();
        set => _httpContextAccessor.HttpContext?.SetIdentityContext(value);
    }
}