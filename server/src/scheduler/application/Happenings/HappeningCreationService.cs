using Meets.Common.Domain;

namespace Meets.Scheduler.Happenings;

internal sealed class HappeningCreationService : IHappeningCreationService
{
    private readonly IHappeningCreationManager _happeningCreationManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public HappeningCreationService(
        IHappeningCreationManager happeningCreationManager,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _happeningCreationManager = happeningCreationManager
            ?? throw new ArgumentNullException(nameof(happeningCreationManager));
        _unitOfWorkManager = unitOfWorkManager
            ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
    }

    public async Task<HappeningModel> CreateEventAsync(CreateHappeningInput input,
        CancellationToken cancellationToken)
    {
        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();

        var happening = await _happeningCreationManager.CreateEventAsync(
            input.Name,
            (input, @event) =>
            {
                @event.SetDescription(input.Description);
            },
            input,
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return happening.MapToModel();
    }
}
