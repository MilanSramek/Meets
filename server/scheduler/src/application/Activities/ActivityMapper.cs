namespace Meets.Scheduler.Activities;

public static class ActivityMapper
{
    public static ActivityModel MapToModel(this Activity activity) => new(
        activity.Id,
        activity.Name,
        activity.Description,
        activity.Version);
}
