using System.Buffers;

namespace System;

public static class StringExtensions
{
    private static readonly char[] s_whiteSpaceChars =
    [
        '\u0009', // Character Tabulation (Tab)
        '\u000A', // Line Feed
        '\u000B', // Line Tabulation
        '\u000C', // Form Feed
        '\u000D', // Carriage Return
        '\u0020', // Space
        '\u0085', // Next Line
        '\u00A0', // No-Break Space
        '\u1680', // Ogham Space Mark
        '\u2000', // En Quad
        '\u2001', // Em Quad
        '\u2002', // En Space
        '\u2003', // Em Space
        '\u2004', // Three-Per-Em Space
        '\u2005', // Four-Per-Em Space
        '\u2006', // Six-Per-Em Space
        '\u2007', // Figure Space
        '\u2008', // Punctuation Space
        '\u2009', // Thin Space
        '\u200A', // Hair Space
        '\u2028', // Line Separator
        '\u2029', // Paragraph Separator
        '\u202F', // Narrow No-Break Space
        '\u205F', // Medium Mathematical Space
        '\u3000'  // Ideographic Space
    ];


    public static string ToCamelCaseInvariant(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Range[]? wordRangeBuffer = null;
        char[]? charBuffer = null;
        try
        {
            int rawWordsCount = value.Count(char.IsWhiteSpace) + 1;
            wordRangeBuffer = ArrayPool<Range>.Shared.Rent(rawWordsCount);
            var wordRanges = wordRangeBuffer.AsSpan(0, rawWordsCount);

            var valueSpan = value.AsSpan();
            int wordsCount = valueSpan.SplitAny(wordRanges, s_whiteSpaceChars,
                StringSplitOptions.RemoveEmptyEntries);

            charBuffer = ArrayPool<char>.Shared.Rent(value.Length);
            var buffer = charBuffer.AsSpan(0, value.Length);

            int index = 0;
            foreach (var wordRange in wordRanges[..wordsCount])
            {
                var word = valueSpan[wordRange];
                buffer[index++] = char.ToUpperInvariant(word[0]);
                word[1..].CopyTo(buffer[index..]);
                index += word.Length - 1;
            }

            buffer[0] = char.ToLowerInvariant(buffer[0]);
            return new string(buffer[0..index]);
        }
        finally
        {
            if (wordRangeBuffer is not null)
            {
                ArrayPool<Range>.Shared.Return(wordRangeBuffer);
            }
            if (charBuffer is not null)
            {
                ArrayPool<char>.Shared.Return(charBuffer);
            }
        }

    }
}