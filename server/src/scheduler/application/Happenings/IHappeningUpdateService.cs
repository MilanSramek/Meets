namespace Meets.Scheduler.Happenings;

public interface IHappeningUpdateService
{
    public Task<HappeningModel> UpdateEventAsync(Guid id, UpdateHappeningInput input,
        CancellationToken cancellationToken);
}
