namespace Meets.Common.Domain;

public interface IIntegrationEventSubscribePoint
{
    public IDisposable Subscribe<TEvent>(
        IIntegrationEventHandler<TEvent> handler);
}
