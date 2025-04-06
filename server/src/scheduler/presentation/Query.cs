using Meets.Common.Domain;
using Meets.Scheduler.Happenings;

namespace Meets.Scheduler;

internal sealed class Query
{
    public async Task<HappeningModel> GetHappeningAsync(
        Guid id,
        [Service] IReadOnlyRepository<Happening, Guid> happenings,
        CancellationToken cancellationToken)
    {
        var happening = await happenings.GetAsync(id, cancellationToken);  // ToDo: DataLoader
        return happening.MapToModel();
    }

    // ToDo: Remove
    public async Task<IEnumerable<HappeningModel>> GetHappeningsAsync(
        [Service] IReadOnlyRepository<Happening, Guid> happenings,
        CancellationToken cancellationToken)
    {
        var happening = await happenings.ToListAsync(cancellationToken);
        return happening
            .Select(HappeningMapper.MapToModel);
    }
}
