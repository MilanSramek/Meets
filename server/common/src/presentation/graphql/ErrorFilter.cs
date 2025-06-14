using HotChocolate;

using Meets.Common.Application.Identity;
using Meets.Common.Domain;

namespace Meets.Common.Presentation.GraphQL;

public sealed class ErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.Exception switch
        {
            UnauthenticatedException => new ErrorBuilder()
                .SetMessage("Authentication is required")
                .SetCode("UNAUTHORIZED")
                .Build(),
            UnauthorizedException => new ErrorBuilder()
                .SetMessage("Current user is not allowed to access this resource")
                .SetCode("FORBIDDEN")
                .Build(),
            BusinessException exception => new ErrorBuilder()
                .SetMessage(exception.Message)
                .SetCode(exception.Code)
                .SetExtensionsByExceptionData(exception.Data)
                .Build(),
            _ => error
        };
    }
}