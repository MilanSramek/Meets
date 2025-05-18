using HotChocolate.Subscriptions;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Common.Presentation.GraphQL.Subscriptions;

public static class Registrations
{
    public static IServiceCollection AddDataWatcherProvider<TData, TKey, TDataEvent>(
        this IServiceCollection services,
        Func<TKey, string> topicPicker,
        Func<IServiceProvider, Func<TKey, TDataEvent?, CancellationToken, ValueTask<TData>>> dataProviderFactory)
        where TKey : notnull
    {
        services
            .AddTransient<IDataWatcherProvider<TData, TKey>>(provider =>
            {
                var receiver = provider.GetRequiredService<ITopicEventReceiver>();
                var dataProvider = dataProviderFactory(provider);
                return new DataWatcherProvider<TData, TKey, TDataEvent>(
                    topicPicker,
                    dataProvider,
                    receiver);
            });
        return services;
    }

    public static IServiceCollection AddObserverTopicSender<TObservedEvent, TTopicEvent>(
        this IServiceCollection services,
        Func<IServiceProvider, Func<TObservedEvent, CancellationToken, ValueTask<TTopicEvent>>> eventConverterFactory,
        Func<TObservedEvent, TTopicEvent, string> topicPicker)
    {
        return services.AddSingleton<IObserverTopicSender<TObservedEvent, TTopicEvent>>(provider =>
        {
            var sender = provider.GetRequiredService<ITopicEventSender>();
            var eventConverter = eventConverterFactory(provider);
            return new ObserverTopicSender<TObservedEvent, TTopicEvent>(
                sender,
                eventConverter,
                topicPicker);
        });
    }

    public static IServiceCollection AddObserverTopicSender<TObservedEvent, TTopicEvent>(
        this IServiceCollection services,
        Func<TObservedEvent, TTopicEvent> eventConverter,
        Func<TObservedEvent, TTopicEvent, string> topicPicker)
    {
        return services.AddObserverTopicSender(
            _ => (@event, _) => ValueTask.FromResult(eventConverter(@event)),
            topicPicker);
    }
}
