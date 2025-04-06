
using Meets.Common.Tools.Observables;

namespace Meets.Common.Presentation.Graphql.Subscriptions;

public interface IObserverTopicSender<TObservedEvent, TTopicEvent> : IAsyncObserver<TObservedEvent>
{
}
