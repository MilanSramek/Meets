using Meets.Common.Domain;

namespace Meets.Scheduler.Activities;

internal sealed class ActivityUpdateService : IActivityUpdateService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public ActivityUpdateService(
        IActivityRepository activityRepository,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _activityRepository = activityRepository
            ?? throw new ArgumentNullException(nameof(activityRepository));
        _unitOfWorkManager = unitOfWorkManager
            ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
    }

    public async Task<ActivityModel> UpdateActivityAsync(
        Guid id,
        UpdateActivityModel input,
        CancellationToken cancellationToken)
    {
        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();

        var activity = await _activityRepository.GetAsync(id, cancellationToken);
        if (input.Name.HasValue)
        {
            activity.SetName(input.Name.Value);
        }
        if (input.Description.HasValue)
        {
            activity.SetDescription(input.Description.Value);
        }
        await _activityRepository.UpdateAsync(activity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return activity.MapToModel();
    }
}
