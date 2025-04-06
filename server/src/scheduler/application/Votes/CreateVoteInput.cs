namespace Meets.Scheduler.Votes;

public sealed record CreateVoteInput(
    Guid PollId,
    IEnumerable<CreateUpdateVoteItemInput> Items);
