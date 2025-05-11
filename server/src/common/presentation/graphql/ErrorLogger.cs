using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Processing;
using HotChocolate.Resolvers;

using Microsoft.Extensions.Logging;

namespace Meets.Common.Presentation.GraphQL;

public sealed class ErrorLogger : ExecutionDiagnosticEventListener
{
    private readonly ILogger _logger;

    public ErrorLogger(ILogger<ErrorLogger> logger)
    {
        _logger = logger;
    }

    public override void ResolverError(
        IMiddlewareContext context,
        IError error)
    {
        LogError(error, context.Operation);
        base.ResolverError(context, error);
    }

    public override void ResolverError(
        IRequestContext context,
        ISelection selection,
        IError error)
    {
        LogError(error, context.Operation);
        base.ResolverError(context, selection, error);
    }

    public override void TaskError(IExecutionTask task, IError error)
    {
        _logger.LogError(
          error.Exception,
          "An error occurred while executing a task at path '{path}'",
          error.Path);
        base.TaskError(task, error);
    }

    public override void RequestError(IRequestContext context, Exception exception)
    {
        _logger.LogError(
            exception,
            "An error occurred while processing request '{requestName}'",
            context.Operation?.Name ?? "<<anonymous>>");
        base.RequestError(context, exception);
    }

    private void LogError(IError error, IOperation? operation)
    {
        _logger.LogError(
            error.Exception,
            "An error occurred while processing request '{requestName}' at path '{path}'",
            operation?.Name ?? "<<anonymous>>",
            error.Path);
    }
}
