using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq;

public static class QueryableExtensions
{

    public static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IQueryable<T> queryable)
    {
        ArgumentNullException.ThrowIfNull(queryable);
        return queryable is IAsyncEnumerable<T> asyncEnumerable
            ? asyncEnumerable
            : throw new InvalidOperationException("The queryable does not implement IAsyncEnumerable interface.");
    }

    public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> queryable,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(queryable);

        List<T> result = [];
        await foreach (var item in queryable.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            result.Add(item);
        }

        return result;
    }

    public static Task<T?> FirstOrDefaultAsync<T>(this IQueryable<T> queryable,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(queryable);
        var queryProvider = queryable.GetAsyncQueryableProvider();

        var expression = Expression.Call(
            GetMethodInfo(Queryable.FirstOrDefault, queryable),
            queryable.Expression);
        return queryProvider.ExecuteAsync<T?>(expression, cancellationToken);
    }

    public static Task<T?> FirstOrDefaultAsync<T>(this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(queryable);
        var queryProvider = queryable.GetAsyncQueryableProvider();

        var expression = Expression.Call(
            GetMethodInfo(Queryable.FirstOrDefault, queryable, predicate),
            queryable.Expression,
            Expression.Quote(predicate));
        return queryProvider.ExecuteAsync<T?>(expression, cancellationToken);
    }

    public static Task<T> FirstAsync<T>(this IQueryable<T> queryable,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(queryable);
        var queryProvider = queryable.GetAsyncQueryableProvider();

        var expression = Expression.Call(
            GetMethodInfo(Queryable.First, queryable),
            queryable.Expression);
        return queryProvider.ExecuteAsync<T>(expression, cancellationToken);
    }

    public static Task<T> FirstAsync<T>(this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(queryable);
        var queryProvider = queryable.GetAsyncQueryableProvider();

        var expression = Expression.Call(
            GetMethodInfo(Queryable.First, queryable, predicate),
            queryable.Expression,
            Expression.Quote(predicate));
        return queryProvider.ExecuteAsync<T>(expression, cancellationToken);
    }

    private static IAsyncQueryProvider GetAsyncQueryableProvider(this IQueryable queryable)
    {
        return queryable.Provider is IAsyncQueryProvider asyncQueryProvider
            ? asyncQueryProvider
            : throw new InvalidOperationException("The query provider does not implement IAsyncQueryableProvider interface.");
    }

    private static MethodInfo GetMethodInfo<T1, T2>(Func<T1, T2> func, T1 _1) => func
        .GetMethodInfo();

    private static MethodInfo GetMethodInfo<T1, T2, T3>(Func<T1, T2, T3> func, T1 _1, T2 _2) => func
        .GetMethodInfo();
}
