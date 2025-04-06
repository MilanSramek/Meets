namespace Meets.Scheduler.Happenings;

public interface IHappeningCreationService
{
    public Task<HappeningModel> CreateEventAsync(CreateHappeningInput input,
        CancellationToken cancellationToken);
}
