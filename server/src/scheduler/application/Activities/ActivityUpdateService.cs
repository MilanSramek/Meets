using Meets.Common.Domain;

namespace Meets.Scheduler.Activities;

internal sealed class ActivityUpdateService : IActivityUpdateService
{
    private readonly IActivityRepository _ActivityRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public ActivityUpdateService(
        IActivityRepository ActivityRepository,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _ActivityRepository = ActivityRepository
            ?? throw new ArgumentNullException(nameof(ActivityRepository));
        _unitOfWorkManager = unitOfWorkManager
            ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
    }

    public async Task<ActivityModel> UpdateActivityAsync(
        Guid id,
        UpdateActivityInput input,
        CancellationToken cancellationToken)
    {
        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();

        var activity = await _ActivityRepository.GetAsync(id, cancellationToken);
        if (input.Name.HasValue)
        {
            activity.SetName(input.Name.Value);
        }
        if (input.Description.HasValue)
        {
            activity.SetDescription(input.Description.Value);
        }
        await _ActivityRepository.UpdateAsync(activity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return activity.MapToModel();
    }
}
