namespace Meets.Scheduler.Happenings;

public static class HappeningMapper
{
    public static HappeningModel MapToModel(this Happening @event) => new(
        @event.Id,
        @event.Name,
        @event.Description,
        @event.Version);
}
