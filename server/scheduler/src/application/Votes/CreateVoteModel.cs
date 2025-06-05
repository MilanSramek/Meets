namespace Meets.Scheduler.Votes;

public sealed record CreateVoteModel(
    Guid PollId,
    IEnumerable<CreateUpdateVoteItemModel> Items);
