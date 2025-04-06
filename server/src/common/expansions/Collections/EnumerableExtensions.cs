namespace Meets.Common.Expansions.Collections;

public static class EnumerableExtensions
{
    public static IReadOnlyCollection<T> EvaluateToReadOnly<T>(this IEnumerable<T> source)
    {
        return source switch
        {
            IReadOnlyCollection<T> collection => collection,
            { } => [.. source],
            _ => throw new ArgumentNullException(nameof(source)),
        };
    }
}
