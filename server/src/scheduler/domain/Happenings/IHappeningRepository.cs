namespace Meets.Scheduler.Happenings;

public interface IHappeningRepository :
    IInsertRepository<Happening, Guid>,
    IUpdateRepository<Happening, Guid>
{
}
