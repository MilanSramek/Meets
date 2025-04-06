using HotChocolate.Execution;

namespace Meets.Scheduler.Happenings;

internal delegate ValueTask<ISourceStream> HappeningWatcherProvider(
    Guid happeningId,
    CancellationToken cancellationToken);
