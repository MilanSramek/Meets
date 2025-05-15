using Meets.Common.Tools.Disposables;
using Meets.Scheduler.Activities;
using Meets.Scheduler.Polls;

namespace Meets.Scheduler;

public static class Initializations
{
    public static IAsyncDisposable InitializeGraphQLPresentation(this IServiceProvider provider)
    {
        return Disposable.Combine(
            provider.InitializePollWatch(),
            provider.InitializeActivityWatch());
    }
}
