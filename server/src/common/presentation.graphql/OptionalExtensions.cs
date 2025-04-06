using HotChocolate;

using Meets.Common.Application;

namespace Meets.Common.Presentation.Graphql;

public static class OptionalExtensions
{
    public static Opt<T> ToOpt<T>(this Optional<T> value)
    {
        return value.HasValue
            ? new(value.Value)
            : Opt<T>.Empty();
    }
}
