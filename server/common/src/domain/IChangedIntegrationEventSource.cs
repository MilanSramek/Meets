namespace Meets.Common.Domain;

public interface IChangedIntegrationEventSource
{
    public object GetChangedEvent();
}
