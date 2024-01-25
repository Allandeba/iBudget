using System.Text.RegularExpressions;
using System.Text;

namespace iBudget.Framework.Extensions;

public static class StringExtensions
{
    public static string Unaccent(this string content)
    {
        string normalized = Regex.Replace(
            content.Normalize(NormalizationForm.FormD),
            @"[\p{Mn}]",
            ""
        );
        return normalized.Normalize(NormalizationForm.FormC);
    }
}
