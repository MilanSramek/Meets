using HotChocolate.Execution;

namespace Meets.Scheduler.Polls;

internal delegate ValueTask<ISourceStream> PollWatcherProvider(
    Guid pollId,
    CancellationToken cancellationToken);
