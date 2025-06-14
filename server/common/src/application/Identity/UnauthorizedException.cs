namespace Meets.Common.Application.Identity;

public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("User is not authorized")
    {
    }

    public UnauthorizedException(string? message) : base(message)
    {
    }
}