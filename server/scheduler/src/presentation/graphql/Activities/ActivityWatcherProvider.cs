using HotChocolate.Execution;

namespace Meets.Scheduler.Activities;

internal delegate ValueTask<ISourceStream> ActivityWatcherProvider(
    Guid ActivityId,
    CancellationToken cancellationToken);
