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
            "An error occurred while processing request '{operationName}' (ID = {operationId})",
            context.Operation?.Name ?? "<<anonymous>>",
            context.Operation?.Id ?? "<<unknown>>");
        base.RequestError(context, exception);
    }

    public override void SubscriptionEventError(SubscriptionEventContext context,
        Exception exception)
    {
        _logger.LogError(
            exception,
            "An error occurred while producing event result of subscription '{operationName}' (ID = {operationId})",
            context.Subscription.Operation?.Name ?? "<<anonymous>>",
            context.Subscription.Operation?.Id ?? "<<unknown>>");
        base.SubscriptionEventError(context, exception);
    }

    public override void SubscriptionTransportError(ISubscription subscription,
        Exception exception)
    {
        _logger.LogError(
            exception,
            "An error occurred while transporting event result of subscription '{operationName}' (ID = {operationId})",
            subscription.Operation?.Name ?? "<<anonymous>>",
            subscription.Operation?.Id ?? "<<unknown>>");
        base.SubscriptionTransportError(subscription, exception);
    }

    private void LogError(IError error, IOperation? operation)
    {
        _logger.LogError(
            error.Exception,
            "An error occurred while resolving '{path}' for operation '{operationName}' (ID = {operationId}): {message}",
            error.Path,
            operation?.Name ?? "<<anonymous>>",
            operation?.Id ?? "<<unknown>>",
            error.Message ?? "<<no message>>");
    }
}
