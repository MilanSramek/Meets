using Meets.Common.Application.Identity;

namespace Meets.Common.Infrastructure.Identity;

public interface IIdentityContextAccessor
{
    public IIdentityContext? Context { get; set; }
}