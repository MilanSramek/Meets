namespace Meets.Common.Domain;

public interface IUnitOfWorkManager
{
    public IUnitOfWork? Current { get; }

    public ValueTask<IUnitOfWork> BeginAsync();
}
