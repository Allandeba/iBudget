using System.Text;
using System.Text.RegularExpressions;

namespace iBudget.Framework.Extensions;

public static class StringExtensions
{
    public static string Unaccent(this string content)
    {
        var normalized = Regex.Replace(
            content.Normalize(NormalizationForm.FormD),
            @"[\p{Mn}]",
            ""
        );
        return normalized.Normalize(NormalizationForm.FormC);
    }

    public static string OnlyNumbers(this string content)
    {
        return Regex.Replace(content, @"[^\d]", "");
    }
}