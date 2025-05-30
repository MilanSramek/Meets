using HotChocolate;

using Meets.Common.Domain;

namespace Meets.Common.Presentation.GraphQL;

public sealed class ErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.Exception switch
        {
            BusinessException exception => new ErrorBuilder()
                .SetMessage(exception.Message)
                .SetCode(exception.Code)
                .SetExtensions(exception.Data)
                .Build(),
            _ => error
        };
    }
}