using Meets.Common.Domain;

namespace Meets.Scheduler.Votes;

internal sealed class VoteService : IVoteCreationService, IVoteUpdateService
{
    private readonly IVoteRepository _voteRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public VoteService(IVoteRepository voteRepository, IUnitOfWorkManager unitOfWorkManager)
    {
        _voteRepository = voteRepository
            ?? throw new ArgumentNullException(nameof(voteRepository));
        _unitOfWorkManager = unitOfWorkManager
            ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
    }

    public async Task<VoteModel> CreateVoteAsync(CreateVoteInput input,
        CancellationToken cancellationToken)
    {
        var vote = new Vote(input.PollId)
            .SetItems(input.Items
                .Select(_ => new VoteItem(_.Date, _.Statement)));

        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();

        await _voteRepository.InsertAsync(vote, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return vote.MapToModel();
    }

    public async Task<VoteModel> UpdateVoteAsync(
        Guid id,
        IEnumerable<CreateUpdateVoteItemInput> items,
        CancellationToken cancellationToken)
    {
        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();

        var vote = await _voteRepository.GetAsync(id, cancellationToken);
        vote.SetItems(items
                .Select(_ => new VoteItem(_.Date, _.Statement)));
        await _voteRepository.UpdateAsync(vote, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return vote.MapToModel();
    }
}
