using Meets.Common.Domain;

namespace Meets.Common.Infrastructure;

public interface IUnitOfWorkAccessor
{
    public IUnitOfWork? Current { get; set; }
}
