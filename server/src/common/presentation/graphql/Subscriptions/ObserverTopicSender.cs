using HotChocolate.Subscriptions;

namespace Meets.Common.Presentation.GraphQL.Subscriptions;

internal sealed class ObserverTopicSender<TObservedEvent, TTopicEvent>
    : IObserverTopicSender<TObservedEvent, TTopicEvent>
{
    private readonly ITopicEventSender _sender;

    private readonly Func<TObservedEvent, CancellationToken, ValueTask<TTopicEvent>> _eventConverter;
    private readonly Func<TObservedEvent, TTopicEvent, string> _topicPicker;

    public ObserverTopicSender(
        ITopicEventSender sender,
        Func<TObservedEvent, CancellationToken, ValueTask<TTopicEvent>> eventConverter,
        Func<TObservedEvent, TTopicEvent, string> topicPicker)
    {
        _sender = sender
            ?? throw new ArgumentNullException(nameof(sender));
        _eventConverter = eventConverter
            ?? throw new ArgumentNullException(nameof(eventConverter));
        _topicPicker = topicPicker
            ?? throw new ArgumentNullException(nameof(topicPicker));
    }

    public async ValueTask OnNextAsync(TObservedEvent observedEvent,
        CancellationToken cancellationToken)
    {
        var topicEvent = await _eventConverter(observedEvent, cancellationToken);
        string topic = _topicPicker(observedEvent, topicEvent);

        await _sender.SendAsync(topic, topicEvent, cancellationToken);
    }

    public ValueTask OnErrorAsync(Exception _1, CancellationToken _2) => ValueTask
        .CompletedTask;

    public ValueTask OnCompletedAsync(CancellationToken _1) => ValueTask
        .CompletedTask;
}
