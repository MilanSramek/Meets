namespace Meets.Scheduler.Votes;

public sealed record VoteModel(
    Guid Id,
    Guid PollId,
    IEnumerable<VoteItemModel> Items);
