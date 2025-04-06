using Meets.Common.Domain;

namespace Meets.Common.Infrastructure;

internal readonly struct UnitOfWorkDisposer : IDisposable
{
    private readonly IUnitOfWorkAccessor _accessor;
    private readonly IUnitOfWork _ancestor;

    public UnitOfWorkDisposer(IUnitOfWorkAccessor accessor, IUnitOfWork ancestor)
    {
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        _ancestor = ancestor ?? throw new ArgumentNullException(nameof(ancestor));
    }

    public readonly void Dispose() => _accessor.Current = _ancestor;
}
