using System.Text.RegularExpressions;

namespace WebApplication1.Extensions;

public static class StringExtensions
{
    private static readonly Regex HtmlTagRegex = new("<.*?>", RegexOptions.Compiled);

    public static string StripHtml(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var result = HtmlTagRegex.Replace(input, string.Empty);
        return result.Trim();
    }

    public static string StripHtmlAndTrim(this string? input, int maxLength)
    {
        var plainText = StripHtml(input);
        if (plainText.Length <= maxLength)
        {
            return plainText;
        }

        return plainText[..maxLength].TrimEnd() + "...";
    }
}
