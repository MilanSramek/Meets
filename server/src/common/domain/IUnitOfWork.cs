namespace Meets.Common.Domain;

public interface IUnitOfWork : IAsyncDisposable
{
    public ValueTask SaveChangesAsync(CancellationToken cancellationToken);

    public ValueTask RollbackAsync(CancellationToken cancellationToken);

    public ValueTask CompleteAsync(CancellationToken cancellationToken);
}
