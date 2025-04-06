namespace Meets.Common.Domain;

public interface IDeletedIntegrationEventSource
{
    public object GetDeletedEvent();
}
