using Meets.Common.Tools.Observables;

namespace Meets.Common.Presentation.GraphQL.Subscriptions;

public interface IObserverTopicSender<TObservedEvent, TTopicEvent> : IAsyncObserver<TObservedEvent>
{
}
