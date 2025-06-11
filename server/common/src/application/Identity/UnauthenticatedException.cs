namespace Meets.Common.Application.Identity;

public class UnauthenticatedException : Exception
{
    public UnauthenticatedException() : base()
    {
    }

    public UnauthenticatedException(string? message) : base(message)
    {
    }
}