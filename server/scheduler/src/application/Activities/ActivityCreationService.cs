using Meets.Common.Domain;

namespace Meets.Scheduler.Activities;

internal sealed class ActivityCreationService : IActivityCreationService
{
    private readonly IActivityCreationManager _activityCreationManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public ActivityCreationService(
        IActivityCreationManager activityCreationManager,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _activityCreationManager = activityCreationManager
            ?? throw new ArgumentNullException(nameof(activityCreationManager));
        _unitOfWorkManager = unitOfWorkManager
            ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
    }

    public async Task<ActivityModel> CreateActivityAsync(CreateActivityModel input,
        CancellationToken cancellationToken)
    {
        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();

        var activity = await _activityCreationManager.CreateActivityAsync(
            input.Name,
            (input, activity) =>
            {
                activity.SetDescription(input.Description);
                activity.SetOwner(input.OwnerId);
            },
            input,
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return activity.MapToModel();
    }
}
