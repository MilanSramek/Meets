namespace Meets.Scheduler.Polls;

public static class PollMapper
{
    public static PollModel MapToModel(this Poll poll) => new(
        poll.Id,
        poll.HappeningId);
}
