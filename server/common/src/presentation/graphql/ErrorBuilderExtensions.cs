using HotChocolate;

using System.Collections;

namespace Meets.Common.Presentation.GraphQL;

internal static class ErrorBuilderExtensions
{
    public static IErrorBuilder SetExtensions(this IErrorBuilder builder, IDictionary data)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(data);

        foreach (DictionaryEntry entry in data)
        {
            string? key = entry.Key.ToString();
            string? value = entry.Value switch
            {
                Type type => type.Name,
                string str => str,
                object obj => obj.ToString(),
                _ => null
            };

            if (key is { } && value is { })
            {
                builder = builder.SetExtension(key.ToCamelCaseInvariant(), value);
            }
        }
        return builder;
    }
}