using Meets.Common.Domain;

namespace Meets.Scheduler.Happenings;

internal sealed class HappeningUpdateService : IHappeningUpdateService
{
    private readonly IHappeningRepository _happeningRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public HappeningUpdateService(
        IHappeningRepository happeningRepository,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _happeningRepository = happeningRepository
            ?? throw new ArgumentNullException(nameof(happeningRepository));
        _unitOfWorkManager = unitOfWorkManager
            ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
    }

    public async Task<HappeningModel> UpdateEventAsync(
        Guid id,
        UpdateHappeningInput input,
        CancellationToken cancellationToken)
    {
        await using var unitOfWork = await _unitOfWorkManager.BeginAsync();

        var happening = await _happeningRepository.GetAsync(id, cancellationToken);
        if (input.Name.HasValue)
        {
            happening.SetName(input.Name.Value);
        }
        if (input.Description.HasValue)
        {
            happening.SetDescription(input.Description.Value);
        }
        await _happeningRepository.UpdateAsync(happening, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return happening.MapToModel();
    }
}
