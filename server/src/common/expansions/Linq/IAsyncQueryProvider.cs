using System.Linq.Expressions;

namespace System.Linq;

public interface IAsyncQueryProvider : IQueryProvider
{
    public Task<TResult> ExecuteAsync<TResult>(Expression expression,
        CancellationToken cancellationToken);
}
