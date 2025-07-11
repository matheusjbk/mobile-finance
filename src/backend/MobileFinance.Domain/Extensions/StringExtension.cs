using System.Diagnostics.CodeAnalysis;

namespace MobileFinance.Domain.Extensions;
public static class StringExtension
{
    /// <summary>
    /// Extension method to check if a string is not null, empty, or whitespace.
    /// </summary>
    /// <returns>
    /// True if the string is not empty; otherwise, false.
    /// </returns>
    public static bool NotEmpty([NotNullWhen(true)] this string? value) => !string.IsNullOrWhiteSpace(value);
}
