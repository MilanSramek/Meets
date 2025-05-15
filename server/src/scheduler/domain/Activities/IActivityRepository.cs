namespace Meets.Scheduler.Activities;

public interface IActivityRepository :
    IInsertRepository<Activity, Guid>,
    IUpdateRepository<Activity, Guid>
{
}
