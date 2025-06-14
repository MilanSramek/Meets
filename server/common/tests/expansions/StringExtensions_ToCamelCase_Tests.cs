namespace Meets.Common.Expansions_Tests;

public sealed class StringExtensions_ToCamelCase_Tests
{
    [Theory]
    [InlineData("HelloWorld", "helloWorld")]
    [InlineData("Hello World", "helloWorld")]
    [InlineData("HELLO WORLD", "hELLOWORLD")]
    [InlineData("hello world", "helloWorld")]
    [InlineData("Hello  World", "helloWorld")]
    [InlineData("Hello   World", "helloWorld")]
    [InlineData("Hello   ", "hello")]
    [InlineData("  Hello World  ", "helloWorld")]
    [InlineData(" ", "")]
    public void ToCamelCaseInvariant_ShouldConvertToCamelCase(string input, string expected)
    {
        // Act
        string result = input.ToCamelCaseInvariant();

        // Assert
        Assert.Equal(expected, result);
    }
}