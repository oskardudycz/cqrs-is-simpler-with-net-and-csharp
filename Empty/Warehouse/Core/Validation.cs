using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public static class Validation
{
    public static Guid AssertNotEmpty(
        [NotNull] this Guid? value,
        [CallerArgumentExpression("value")] string? argumentName = null
    ) =>
        (value != null && value.Value != Guid.Empty)
            ? value.Value
            : throw new ArgumentOutOfRangeException(argumentName);


    public static string AssertNotEmpty(
        [NotNull] this string? value,
        [CallerArgumentExpression("value")] string? argumentName = null
    ) =>
        !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentOutOfRangeException(argumentName);


    public static string? AssertNullOrNotEmpty(
        this string? value,
        [CallerArgumentExpression("value")] string? argumentName = null
    ) =>
        value?.AssertNotEmpty(argumentName);

    public static string AssertMatchesRegex(
        [NotNull] this string? value,
        [StringSyntax(StringSyntaxAttribute.Regex)]
        string pattern,
        [CallerArgumentExpression("value")] string? argumentName = null
    ) =>
        Regex.IsMatch(value.AssertNotEmpty(), pattern)
            ? value
            : throw new ArgumentOutOfRangeException(argumentName);

    public static int AssertPositive(
        [NotNull] this int? value,
        [CallerArgumentExpression("value")] string? argumentName = null
    ) =>
        value?.AssertPositive() ?? throw new ArgumentOutOfRangeException(argumentName);

    public static int AssertPositive(
        this int value,
        [CallerArgumentExpression("value")] string? argumentName = null
    ) =>
        value > 0
            ? value
            : throw new ArgumentOutOfRangeException(argumentName);
}
