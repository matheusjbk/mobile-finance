using System.Diagnostics.CodeAnalysis;

namespace MobileFinance.Domain.Extensions;
public static class StringExtension
{
    // Extension method to check if a string is not null, empty, or whitespace.  
    public static bool NotEmpty([NotNullWhen(true)] this string? value) => !string.IsNullOrWhiteSpace(value);
}
